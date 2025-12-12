import { useGetCommentsByDocumentIdQuery, useUpdateCommentMutation, useDeleteCommentMutation } from '@/features/document/api/Comment.api';
import Loading from '@/features/shared/components/Loading';
import { useRef, useState } from 'react';
import type { CommentModel } from '../types/CommentModel';

const Comments:React.FC<{ t: (key: string) => string, documentId: string}> = ({ t, documentId}) => {
    const { data: comments, isLoading } = useGetCommentsByDocumentIdQuery(documentId);
    const [updateComment] = useUpdateCommentMutation();
    const [deleteComment] = useDeleteCommentMutation();

    const contentRef = useRef<HTMLTextAreaElement | null>(null);

    const [isEditMode, setIsEditMode] = useState(false);
    const [selectedCommentId, setSelectedCommentId] = useState("");

    const updateHandle = (id: string) => {
        setSelectedCommentId(id);
        setIsEditMode(true);
    }

    const deleteAsync = async (commentId: string) => {
            try {
                const agrs = { documentId, commentId }
                await deleteComment(agrs).unwrap();
            } catch (e) {
                console.log(e);
            }
        }
    
    const updateAsync = async (selectedDocument: CommentModel) => {
        try {
            const document: CommentModel = {
                id: selectedDocument.id,
                content: contentRef.current?.value ?? "",
                documentId: documentId,
                userId: selectedDocument.userId
            };
                
            await updateComment(document).unwrap();
    
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
        {comments?.length === 0
            ? <div>{t("NoAnyComments")}</div>
            : <ul className="comments">{comments?.map((comment) => (
                <li key={comment.id} className="container">
                    {(isEditMode && selectedCommentId === comment.id)
                        ?
                            <div className="comments__item">
                                <textarea rows={6} defaultValue={comment.content} ref={contentRef} />
                                <div className="actions">
                                    <button className="btn-border-shadow" onClick={async () => await updateAsync(comment)}>{t("Save")}</button>
                                    <button className="btn-border-shadow" onClick={() => setIsEditMode(false)}>{t("Cancel")}</button>
                                </div>
                            </div>
                        :
                            <div className="comments__item">
                                <div>{t("Content")}: {comment.content}</div>
                                <div className="actions">
                                    <button className="btn-border-shadow" onClick={async () => await deleteAsync(comment.id)}>{t("Delete")}</button>
                                    <button className="btn-border-shadow" onClick={() => updateHandle(comment.id)}>{t("Update")}</button>
                                </div>
                            </div>
                    }
                </li>
            ))}
            </ul>
        }
        </>
    );
}

export default Comments;