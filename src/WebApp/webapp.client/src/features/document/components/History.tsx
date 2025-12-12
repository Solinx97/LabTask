import { useGetHistoryDocumentsByUserIdQuery } from '@/features/document/api/Document.api';
import Loading from '@/features/shared/components/Loading';
import { useState } from 'react';
import Comments from './Comments';

const History:React.FC<{ t: (key: string) => string, userId: string}> = ({ t, userId }) => {
    const { data: myHistoryDocuments, isLoading } = useGetHistoryDocumentsByUserIdQuery(userId);

    const [isOpenComments, setIsOpenComments] = useState(false);
    const [selectedDocumentId, setSelectedDocumentId] = useState("");

    const commentsHandle = (id?: string) => {
        setSelectedDocumentId(id ?? "");
        setIsOpenComments((prev) => !prev);
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

    if (isLoading) {
        return (<Loading />);
    }

    return (
        <>
        {myHistoryDocuments?.length === 0
            ? <div>{t("NoAnyDocuments")}</div>
            : <ul className="documents">{myHistoryDocuments?.map((document) => (
                <li key={document.id} className="container">
                    <div className="documents__item">
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
                </li>
            ))}
            </ul>
        }
        </>
    );
}

export default History;