import { useState } from 'react';
import type { DocumentModel } from '../types/DocumentModel';
import Comments from './Comments';

interface Props {
    t: (key: string) => string;
    document: DocumentModel;
    getTime: (dateAsString: string) => string;
}

const RestrictedDocument: React.FC<Props> = ({ t, document, getTime }) => {
    const [isOpenComments, setIsOpenComments] = useState(false);
    const [selectedDocumentId, setSelectedDocumentId] = useState("");

    const commentsHandle = (id?: string) => {
        setSelectedDocumentId(id ?? "");
        setIsOpenComments((prev) => !prev);
    }

    return (
        <>
            <div className="container">
                <div>{t("Name")}: {document.name}</div>
                <div>{t("Description")}: {document.description}</div>
                <div>{t("ExpireAt")}: {getTime(document.expireAt)}</div>
                <div className="actions">
                    <button className={`btn-border-shadow ${isOpenComments && selectedDocumentId === document.id ? 'green' : ''}`} onClick={() => commentsHandle(document.id)}>{t("Comments")}</button>
                </div>
            </div>
            {(isOpenComments && selectedDocumentId === document.id) &&
                <Comments
                    t={t}
                    documentId={document.id}
                    actionsAllow={false}
                />
            }
        </>
    );
}

export default RestrictedDocument;