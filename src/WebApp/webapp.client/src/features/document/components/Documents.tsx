import { useGetDocumentsByUserIdQuery, useDeleteDocumentMutation, useUpdateDocumentMutation } from '@/features/document/api/Dcoument.api';
import Loading from '@/features/shared/components/Loading';
import { useRef, useState } from 'react';
import type { DocumentModel } from '../types/DocumentModel';
import AddComment from './AddComment';

const Documents:React.FC<{ t: (key: string) => string, userId: string}> = ({ t, userId }) => {
    const { data: myDocuments, isLoading } = useGetDocumentsByUserIdQuery(userId);
    const [deleteDocument] = useDeleteDocumentMutation();
    const [updateDocument] = useUpdateDocumentMutation();

    const nameRef = useRef<HTMLInputElement | null>(null);
    const descriptiondRef = useRef<HTMLTextAreaElement | null>(null);
    
    const [isEditMode, setIsEditMode] = useState(false);
    const [isOpenAddComment, setIsOpenAddComment] = useState(false);
    const [selectedDocumentId, setSelectedDocumentId] = useState("");

    const updateHandle = (id: string) => {
        setSelectedDocumentId(id);
        setIsEditMode(true);
    }

    const openAddComment = (id: string) => {
        setSelectedDocumentId(id);
        setIsOpenAddComment(true);
    }

    const closeAddComment = () => {
        setSelectedDocumentId("");
        setIsOpenAddComment(false);
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

    if (isLoading) {
        return (<Loading />);
    }

    return (
        <>
        {myDocuments?.length === 0
            ? <div>{t("NoAnyDocuments")}</div>
            : <ul className="documents">{myDocuments?.map((document) => (
                <li key={document.id}>
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
                                <div className="actions">
                                    <button className="btn-border-shadow" onClick={async () => await deleteAsync(document.id)}>{t("Delete")}</button>
                                    <button className="btn-border-shadow" onClick={() => updateHandle(document.id)}>{t("Update")}</button>
                                </div>
                                {(isOpenAddComment && selectedDocumentId === document.id)
                                    ? <div className="open-comments" onClick={() => closeAddComment()}>{t("Cancel")}</div>
                                    : <div className="open-comments" onClick={() => openAddComment(document.id)}>{t("AddComment")}</div>
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
                </li>
            ))}
            </ul>
        }
        </>
    );
}

export default Documents;