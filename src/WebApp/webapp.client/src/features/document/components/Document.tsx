import { useDeleteDocumentMutation, useUpdateDocumentMutation } from '@/features/document/api/Document.api';
import { useRef, useState } from 'react';
import type { DocumentModel } from '../types/DocumentModel';
import AddComment from './AddComment';
import Comments from './Comments';

interface Props {
    t: (key: string) => string;
    userId: string;
    document: DocumentModel;
    getTime: (dateAsString?: string) => string;
    getDocumentLinks?: (document: DocumentModel | null) => void;
    commentsAllow?: boolean;
    addCommentsAllow?: boolean;
    updateAllow?: boolean;
    deleteAllow?: boolean;
    commentActionsAllow?: boolean;
    linksAllow?: boolean;
}

const Document: React.FC<Props> = ({ 
    t, userId, document,
    getTime, getDocumentLinks, commentsAllow = true, 
    addCommentsAllow = true, updateAllow = true, deleteAllow = true,
    commentActionsAllow = true, linksAllow = true 
}) => {
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

    const getDocumentLinksHandler = () => {
        if (getDocumentLinks) {
            getDocumentLinks(document);
        }
    }

    return (
        <>
            {(isEditMode && selectedDocumentId === document.id)
                ?
                <div className="container">
                    <input type="text" defaultValue={document.name} ref={nameRef} />
                    <textarea rows={6} defaultValue={document.description} ref={descriptiondRef} />
                    <div className="actions">
                        <button className="btn-border-shadow" onClick={async () => await updateAsync(document)}>{t("Save")}</button>
                        <button className="btn-border-shadow" onClick={() => setIsEditMode(false)}>{t("Cancel")}</button>
                    </div>
                </div>
                :
                <div className="container">
                    {linksAllow &&
                        <button className="btn-border-shadow links" onClick={getDocumentLinksHandler}>{t("Links")}</button>
                    }
                    <div className="title" title={document.name}>{document.name}</div>
                    <div className="description">{document.description}</div>
                    <div className="expire-at">{getTime(document.expireAt)}</div>
                    <div className="actions">
                        {deleteAllow &&
                            <button className="btn-border-shadow" onClick={async () => await deleteAsync(document.id)}>{t("Delete")}</button>
                        }
                        {updateAllow &&
                            <button className="btn-border-shadow" onClick={() => updateHandle(document.id)}>{t("Update")}</button>
                        }
                        {commentsAllow &&
                            <button className={`btn-border-shadow ${isOpenComments && selectedDocumentId === document.id ? 'green' : ''}`} onClick={() => commentsHandle(document.id)}>{t("Comments")}</button>
                        }
                    </div>
                    {addCommentsAllow &&
                        <>
                            {(isOpenAddComment && selectedDocumentId === document.id)
                                ? <div className="open-comments" onClick={() => addCommentHandle()}>{t("Cancel")}</div>
                                : <div className="open-comments" onClick={() => addCommentHandle(document.id)}>{t("AddComment")}</div>
                            }
                        </>
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
                    userId={userId}
                    actionsAllow={commentActionsAllow}
                />
            }
        </>
    );
}

export default Document;