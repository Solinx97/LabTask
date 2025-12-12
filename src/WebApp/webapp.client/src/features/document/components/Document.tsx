import { useDeleteDocumentMutation, useUpdateDocumentMutation } from '@/features/document/api/Document.api';
import { useRef, useState } from 'react';
import type { DocumentModel } from '../types/DocumentModel';
import AddComment from './AddComment';
import Comments from './Comments';

const Document: React.FC<{ t: (key: string) => string, userId: string, document: DocumentModel }> = ({ t, userId, document }) => {
    const [deleteDocument] = useDeleteDocumentMutation();
    const [updateDocument] = useUpdateDocumentMutation();

    const nameRef = useRef<HTMLInputElement | null>(null);
    const descriptiondRef = useRef<HTMLTextAreaElement | null>(null);

    const [isEditMode, setIsEditMode] = useState(false);
    const [isOpenAddComment, setIsOpenAddComment] = useState(false);
    const [isOpenComments, setIsOpenComments] = useState(false);
    const [selectedDocumentId, setSelectedDocumentId] = useState("");

    const updateHandle = (id: string) => {
        setSelectedDocumentId(id);
        setIsEditMode(true);
    }

    const commentsHandle = (id?: string) => {
        setSelectedDocumentId(id ?? "");
        setIsOpenAddComment(false);
        setIsOpenComments((prev) => !prev);
    }

    const addCommentHandle = (id?: string) => {
        setSelectedDocumentId(id ?? "");
        setIsOpenComments(false);
        setIsOpenAddComment((prev) => !prev);
    }

    const deleteAsync = async (id: string) => {
        try {
            await deleteDocument(id).unwrap();
        } catch (e) {
            console.log(e);
        }
    }

    const updateAsync = async (selectedDocument: DocumentModel) => {
        try {
            const document: DocumentModel = {
                id: selectedDocument.id,
                name: nameRef.current ? nameRef.current.value : "",
                description: descriptiondRef.current ? descriptiondRef.current.value : "",
                expireAt: selectedDocument.expireAt,
                userId: selectedDocument.userId
            };

            await updateDocument(document).unwrap();

            setIsEditMode(false);
        } catch (e) {
            console.log(e);
        }
    }

    const getTime = (dateAsString: string) => {
        const date = new Date(dateAsString);

        const internationalMonth = date.getMonth() + 1;
        const month = internationalMonth < 10 ? `0${internationalMonth}` : internationalMonth;
        const day = date.getDay() < 10 ? `0${date.getDay()}` : date.getDay();
        const hours = date.getHours() < 10 ? `0${date.getHours()}` : date.getHours();
        const minutes = date.getMinutes() < 10 ? `0${date.getMinutes()}` : date.getMinutes();

        const currentDate = `${date.getFullYear()}-${month}-${day} ${hours}:${minutes}:00`;

        return currentDate;
    }

    return (
        <>
            {(isEditMode && selectedDocumentId === document.id)
                ?
                <div className="documents__item">
                    <input type="text" defaultValue={document.name} ref={nameRef} />
                    <textarea rows={6} defaultValue={document.description} ref={descriptiondRef} />
                    <div className="actions">
                        <button className="btn-border-shadow" onClick={async () => await updateAsync(document)}>{t("Save")}</button>
                        <button className="btn-border-shadow" onClick={() => setIsEditMode(false)}>{t("Cancel")}</button>
                    </div>
                </div>
                :
                <div className="documents__item">
                    <div>{t("Name")}: {document.name}</div>
                    <div>{t("Description")}: {document.description}</div>
                    <div>{t("ExpireAt")}: {getTime(document.expireAt)}</div>
                    <div className="actions">
                        <button className="btn-border-shadow" onClick={async () => await deleteAsync(document.id)}>{t("Delete")}</button>
                        <button className="btn-border-shadow" onClick={() => updateHandle(document.id)}>{t("Update")}</button>
                        <button className={`btn-border-shadow ${isOpenComments && selectedDocumentId === document.id ? 'green' : ''}`} onClick={() => commentsHandle(document.id)}>{t("Comments")}</button>
                    </div>
                    {(isOpenAddComment && selectedDocumentId === document.id)
                        ? <div className="open-comments" onClick={() => addCommentHandle()}>{t("Cancel")}</div>
                        : <div className="open-comments" onClick={() => addCommentHandle(document.id)}>{t("AddComment")}</div>
                    }
                </div>
            }
            {(isOpenAddComment && selectedDocumentId === document.id) &&
                <AddComment
                    t={t}
                    userId={userId}
                    documentId={document.id}
                />
            }
            {(isOpenComments && selectedDocumentId === document.id) &&
                <Comments
                    t={t}
                    documentId={document.id}
                />
            }
        </>
    );
}

export default Document;