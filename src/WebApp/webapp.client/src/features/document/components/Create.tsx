import { useCreateDocumentMutation } from '@/features/document/api/Document.api';
import { useRef } from 'react';
import type { DocumentModel } from '../types/DocumentModel';

const Create:React.FC<{ t: (key: string) => string, userId: string }> = ({ t, userId }) => {
    const nameRef = useRef<HTMLInputElement | null>(null);
    const descriptiondRef = useRef<HTMLTextAreaElement | null>(null);
    const expiresAtRef = useRef<HTMLInputElement | null>(null);

    const [createDocument] = useCreateDocumentMutation();

    const createAsync = async () => {
        try {
            const document: DocumentModel = {
                id: crypto.randomUUID(),
                name: nameRef.current ? nameRef.current.value : "",
                description: descriptiondRef.current ? descriptiondRef.current.value : "",
                expireAt: expiresAtRef.current ? expiresAtRef.current.value : "",
                userId: userId
            };

            await createDocument(document).unwrap();
        } catch (e) {
            console.log(e);
        }
    }

    return (
        <div className="create-document">
            <div className="title">{t("Create")}</div>
            <form className="create-document__action" onSubmit={createAsync}>
                <div className="mb-3">
                    <label htmlFor="name">{t("Name")}</label>
                    <input className="form-control" type="text" name="name" placeholder={t("Name")} ref={nameRef} required />
                </div>
                <div className="mb-3">
                    <label htmlFor="description">{t("Description")}</label>
                    <textarea className="form-control" name="description" rows={6} ref={descriptiondRef} required />
                </div>
                <div className="mb-3">
                    <label htmlFor="expires">{t("ExpiresAt")}</label>
                    <input className="form-control" type="date" name="expires" ref={expiresAtRef} required />
                </div>
                <div className="actions">
                    <input type="submit" className="btn-border-shadow" value={t("Create")} />
                </div>
            </form>
        </div>
    );
}

export default Create;