import { useGetActualDocumentsByUserIdQuery } from '@/features/document/api/Document.api';
import Loading from '@/features/shared/components/Loading';
import Document from './Document';
import { useEffect, useRef, useState } from 'react';
import InfiniteScrollTrigger from '@/events/InfiniteScrollTrigger';
import Links from './Links';
import type { DocumentModel } from '../types/DocumentModel';

interface Props {
    t: (key: string) => string;
    userId: string;
    getTime: (dateAsString?: string) => string;
}

const Documents: React.FC<Props> = ({ t, userId, getTime }) => {
    const pageSizeRef = useRef<number>(5);

    const [page, setPage] = useState(0);
    const [hasMore, setHasMore] = useState(false);
    const [isOpenLinks, setIsOpenLinks] = useState(false);
    const [selectedDocument, setSelectedDocument] = useState<DocumentModel | null>(null);

    const { data: documents, isLoading } = useGetActualDocumentsByUserIdQuery({ userId, page: page, pageSize: pageSizeRef.current });

    useEffect(() => {
        if (!documents) {
            return;
        }

        setHasMore((page * pageSizeRef.current) < documents.length);
    }, [page, documents]);

    const getDocumentLinks = (document: DocumentModel | null) => {
        setIsOpenLinks(true);
        setSelectedDocument(document);
    }

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
                            getDocumentLinks={getDocumentLinks}
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
            {isOpenLinks &&
                <Links
                    t={t}
                    getTime={getTime}
                    userId={userId}
                    document={selectedDocument}
                    setIsOpenLinks={setIsOpenLinks}
                />
            }
        </>
    );
}

export default Documents;