import { useCreateDocumentMutation } from '@/features/document/api/Document.api';
import { useRef, useState, type SetStateAction } from 'react';
import type { DocumentModel } from '../types/DocumentModel';

interface Props {
    t: (key: string) => string;
    userId: string;
    setIsOpenCreate: (value: SetStateAction<boolean>) => void;
    getTime: (dateAsString?: string) => string;
}

const Create:React.FC<Props> = ({ t, userId, setIsOpenCreate, getTime }) => {
    const nameRef = useRef<HTMLInputElement | null>(null);
    const descriptiondRef = useRef<HTMLTextAreaElement | null>(null);
    const expiresAtRef = useRef<HTMLInputElement | null>(null);

    const [createDocument] = useCreateDocumentMutation();

    const [isSomeProblems, setIsSomeProblems] = useState(false);

    const createAsync = async (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();

        try {
            if (!nameRef.current || !descriptiondRef.current) {
                return;
            }

            setIsSomeProblems(false);

            const document: DocumentModel = {
                id: crypto.randomUUID(),
                name: nameRef.current ? nameRef.current.value : "",
                description: descriptiondRef.current ? descriptiondRef.current.value : "",
                expireAt: expiresAtRef.current ? expiresAtRef.current.value : "",
                userId: userId
            };

            await createDocument(document).unwrap();

            setIsOpenCreate(false);
        } catch (e) {
            setIsSomeProblems(true);
            console.log(e);
        }
    }

    return (
        <div className="create-document modal-window">
            <div className="title">{t("CreateDocument")}</div>
            <form className="create-document__action" onSubmit={createAsync}>
                <div className="mb-3">
                    <label htmlFor="name">{t("Name")}</label>
                    <input className="form-control" type="text" name="name" ref={nameRef} required />
                </div>
                <div className="mb-3">
                    <label htmlFor="description">{t("Description")}</label>
                    <textarea className="form-control" name="description" rows={6} ref={descriptiondRef} required />
                </div>
                <div className="mb-3">
                    <label htmlFor="expires">{t("ExpiresAt")}</label>
                    <input className="form-control" type="datetime-local" defaultValue={getTime()} name="expires" ref={expiresAtRef} required />
                </div>
                <div className="actions">
                    <input type="submit" className="btn-border-shadow" value={t("Create")} />
                    <input type="button" className="btn-border-shadow orange" value={t("Cancel")} onClick={() => setIsOpenCreate(false)} />
                </div>
                {isSomeProblems &&
                    <div className="alert alert-warning">{t("SomeProblemsDuringCreation")}</div>
                }
            </form>
        </div>
    );
}

export default Create;