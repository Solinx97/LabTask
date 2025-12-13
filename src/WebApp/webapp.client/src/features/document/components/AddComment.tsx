import type { CommentModel } from "../types/CommentModel";
import { useCreateCommentMutation } from '@/features/document/api/Comment.api';
import { useRef, type SetStateAction } from 'react';

interface AddCommentProps {
    t: (key: string) => string;
    userId: string;
    documentId: string;
    setIsOpenAddComment: (value: SetStateAction<boolean>) => void;
}

const AddComment:React.FC<AddCommentProps> = ({ t, userId, documentId, setIsOpenAddComment }) => {
    const contentRef = useRef<HTMLTextAreaElement | null>(null);

    const [createComment] = useCreateCommentMutation();

    const createAsync = async (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();

        try {
            const comment: CommentModel = {
                id: crypto.randomUUID(),
                content: contentRef.current ? contentRef.current.value : "",
                documentId: documentId,
                userId: userId
            };
                
            await createComment(comment).unwrap();

            setIsOpenAddComment(false);
        } catch (e) {
            console.log(e);
        }
    }

    return (
        <div className="add-comment">
            <div>{t("Comment")}</div>
            <form className="add-comment__action" onSubmit={createAsync}>
                <div className="mb-3">
                    <textarea className="form-control" rows={6} ref={contentRef} required />
                </div>
                <div className="actions">
                    <input type="submit" className="btn-border-shadow" value={t("Save")} />
                </div>
            </form>
        </div>
    );
}

export default AddComment;