import { useGetCommentsByDocumentIdQuery, useUpdateCommentMutation, useDeleteCommentMutation } from '@/features/document/api/Comment.api';
import Loading from '@/features/shared/components/Loading';
import { useEffect, useRef, useState } from 'react';
import type { CommentModel } from '../types/CommentModel';
import InfiniteScrollTrigger from '@/events/InfiniteScrollTrigger';

const Comments: React.FC<{ t: (key: string) => string, documentId: string, actionsAllow?: false }> = ({ t, documentId, actionsAllow = true }) => {
    const pageSizeRef = useRef<number>(5);

    const [page, setPage] = useState(0);
    const [hasMore, setHasMore] = useState(false);
    const [comments, setComments] = useState<CommentModel[]>([]);

    const { data: uploadedComments, isLoading } = useGetCommentsByDocumentIdQuery({ documentId, page: page, pageSize: pageSizeRef.current });
    const [updateComment] = useUpdateCommentMutation();
    const [deleteComment] = useDeleteCommentMutation();

    const contentRef = useRef<HTMLTextAreaElement | null>(null);

    const [isEditMode, setIsEditMode] = useState(false);
    const [selectedCommentId, setSelectedCommentId] = useState("");

    useEffect(() => {
        setHasMore((page * pageSizeRef.current) < comments.length);
    }, [page, comments]);

    useEffect(() => {
        if (!uploadedComments) {
            return;
        }

        setComments(prev => prev = [...prev, ...uploadedComments]);
    }, [uploadedComments]);

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
            {comments.length === 0
                ? <div>{t("NoAnyComments")}</div>
                : <ul className="comments">{comments.map((comment) => (
                    <li key={comment.id} className="comments__item">
                        {(isEditMode && selectedCommentId === comment.id)
                            ?
                            <div className="container">
                                <textarea rows={6} defaultValue={comment.content} ref={contentRef} />
                                <div className="actions">
                                    <button className="btn-border-shadow" onClick={async () => await updateAsync(comment)}>{t("Save")}</button>
                                    <button className="btn-border-shadow" onClick={() => setIsEditMode(false)}>{t("Cancel")}</button>
                                </div>
                            </div>
                            :
                            <div className="container">
                                <div>{comment.content}</div>
                                {actionsAllow &&
                                    <div className="actions">
                                        <button className="btn-border-shadow" onClick={async () => await deleteAsync(comment.id)}>{t("Delete")}</button>
                                        <button className="btn-border-shadow" onClick={() => updateHandle(comment.id)}>{t("Update")}</button>
                                    </div>
                                }
                            </div>
                        }
                    </li>
                    ))}
                    <li>
                        <InfiniteScrollTrigger
                            onLoadMore={() => setPage(p => p + 1)}
                            hasMore={hasMore}
                            isLoading={isLoading}
                        />
                    </li>
                </ul>
            }
        </>
    );
}

export default Comments;