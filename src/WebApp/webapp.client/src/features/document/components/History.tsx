import { useGetExpiredDocumentsByUserIdQuery } from '@/features/document/api/Document.api';
import { useEffect, useRef, useState } from 'react';
import Document from './Document';
import InfiniteScrollTrigger from '@/events/InfiniteScrollTrigger';
import Loading from '@/features/shared/components/Loading';

interface Props {
    t: (key: string) => string;
    userId: string;
    getTime: (dateAsString?: string) => string;
}

const History: React.FC<Props> = ({ t, userId, getTime }) => {
    const pageSizeRef = useRef<number>(5);

    const [page, setPage] = useState(0);
    const [hasMore, setHasMore] = useState(false);

    const { data: documents, isLoading } = useGetExpiredDocumentsByUserIdQuery({ userId, page: page, pageSize: pageSizeRef.current });

    useEffect(() => {
        if (!documents) {
            return;
        }

        setHasMore((page * pageSizeRef.current) < documents.length);
    }, [page, documents]);

    if (isLoading || !documents) {
        return (<Loading />);
    }

    return (
        <>
            {documents.length === 0
                ? <div>{t("NoAnyDocuments")}</div>
                : <ul className="documents">{documents.map((document) => (
                    <li key={document.id} className="documents__item">
                        <Document
                            t={t}
                            userId={userId}
                            document={document}
                            getTime={getTime}
                            updateAllow={false}
                            deleteAllow={false}
                            addCommentsAllow={false}
                            commentActionsAllow={false}
                            linksAllow={false}
                        />
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

export default History;