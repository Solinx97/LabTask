import { useGetLinksByUserIdQuery } from '@/features/document/api/Link.api';
import Loading from '@/features/shared/components/Loading';
import { useEffect, useRef, useState } from 'react';
import InfiniteScrollTrigger from '@/events/InfiniteScrollTrigger';
import DocumentByLink from './DocumentByLink';

const Manage: React.FC<{ t: (key: string) => string, userId: string, getTime: (dateAsString?: string) => string }> = ({ t, userId, getTime }) => {
    const pageSizeRef = useRef<number>(5);

    const [page, setPage] = useState(0);
    const [hasMore, setHasMore] = useState(false);

    const { data: links, isLoading } = useGetLinksByUserIdQuery({ userId, page: page, pageSize: pageSizeRef.current });

    useEffect(() => {
        if (!links) {
            return;
        }

        setHasMore((page * pageSizeRef.current) < links.length);
    }, [page, links]);

    if (isLoading || !links) {
        return (<Loading />);
    }

    return (
        <>
            {links.length === 0
                ? <div>{t("NoAnyDocuments")}</div>
                : <ul className="documents">{links.map((link) => (
                    <li key={link.id} className="documents__item">
                        <DocumentByLink
                            t={t}
                            id={link.documentId}
                            userId={userId}
                            getTime={getTime}
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

export default Manage;