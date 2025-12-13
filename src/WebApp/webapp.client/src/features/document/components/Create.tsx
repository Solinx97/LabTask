import { useCreateDocumentMutation } from '@/features/document/api/Document.api';
import { useRef, useState } from 'react';
import type { DocumentModel } from '../types/DocumentModel';

const Create:React.FC<{ t: (key: string) => string, userId: string }> = ({ t, userId }) => {
    const nameRef = useRef<HTMLInputElement | null>(null);
    const descriptiondRef = useRef<HTMLTextAreaElement | null>(null);
    const expiresAtRef = useRef<HTMLInputElement | null>(null);

    const [createDocument] = useCreateDocumentMutation();
    const [created, setCreated] = useState(false);

    const createAsync = async (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();

        try {
            if (!nameRef.current || !descriptiondRef.current) {
                return;
            }

            const document: DocumentModel = {
                id: crypto.randomUUID(),
                name: nameRef.current ? nameRef.current.value : "",
                description: descriptiondRef.current ? descriptiondRef.current.value : "",
                expireAt: expiresAtRef.current ? expiresAtRef.current.value : "",
                userId: userId
            };

            await createDocument(document).unwrap();

            nameRef.current.value = "";
            descriptiondRef.current.value = "";
            setCreated(true);
        } catch (e) {
            console.log(e);
        }
    }

    const getCurrentTime = () => {
        const date = new Date();

        const internationalMonth = date.getMonth() + 1;
        const month = internationalMonth < 10 ? `0${internationalMonth}` : internationalMonth;
        const day = date.getDay() < 10 ? `0${date.getDay()}` : date.getDay();
        const hours = date.getHours() < 10 ? `0${date.getHours()}` : date.getHours();
        const minutes = date.getMinutes() < 10 ? `0${date.getMinutes()}` : date.getMinutes();

        const currentDate = `${date.getFullYear()}-${month}-${day}T${hours}:${minutes}:00`;

        return currentDate;
    }

    return (
        <div className="create-document">
            <div className="title">{t("Create")}</div>
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
                    <input className="form-control" type="datetime-local" defaultValue={getCurrentTime()} name="expires" ref={expiresAtRef} required />
                </div>
                <div className="actions">
                    <input type="submit" className="btn-border-shadow" value={t("Create")} />
                </div>
            </form>
            {created &&
                <div className="alert alert-success" role="alert">{t("Created")}!</div>
            }
        </div>
    );
}

export default Create;