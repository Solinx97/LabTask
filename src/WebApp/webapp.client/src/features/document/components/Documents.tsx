import { useGetActualDocumentsByUserIdQuery } from '@/features/document/api/Document.api';
import Loading from '@/features/shared/components/Loading';
import Document from './Document';
import { useEffect, useRef, useState } from 'react';
import InfiniteScrollTrigger from '@/events/InfiniteScrollTrigger';
import type { DocumentModel } from '../types/DocumentModel';

const Documents: React.FC<{ t: (key: string) => string, userId: string }> = ({ t, userId }) => {
    const pageSizeRef = useRef<number>(5);

    const [page, setPage] = useState(0);
    const [hasMore, setHasMore] = useState(false);
    const [documents, setDocuments] = useState<DocumentModel[]>([]);

    const { data: uploadedDocuments, isLoading } = useGetActualDocumentsByUserIdQuery({ userId, page: page, pageSize: pageSizeRef.current });

    useEffect(() => {
        setHasMore((page * pageSizeRef.current) < documents.length);
    }, [page, documents]);

    useEffect(() => {
        if (!uploadedDocuments) {
            return;
        }

        setDocuments(prev => prev = [...prev, ...uploadedDocuments]);
    }, [uploadedDocuments]);

    if (isLoading) {
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

export default Documents;