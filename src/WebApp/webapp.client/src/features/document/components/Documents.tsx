import { useGetActualDocumentsByUserIdQuery, useLazyGetDocumentByNameQuery } from '@/features/document/api/Document.api';
import Loading from '@/features/shared/components/Loading';
import Document from './Document';
import { useEffect, useRef, useState } from 'react';
import InfiniteScrollTrigger from '@/events/InfiniteScrollTrigger';
import Links from './Links';
import type { DocumentModel } from '../types/DocumentModel';
import Create from './Create';

interface Props {
    t: (key: string) => string;
    userId: string;
    getTime: (dateAsString?: string) => string;
}

const Documents: React.FC<Props> = ({ t, userId, getTime }) => {
    const pageSizeRef = useRef<number>(5);
    const nameRef = useRef<HTMLInputElement | null>(null);

    const [page, setPage] = useState(0);
    const [hasMore, setHasMore] = useState(false);
    const [isOpenLinks, setIsOpenLinks] = useState(false);
    const [isOpenCreate, setIsOpenCreate] = useState(false);
    const [selectedDocument, setSelectedDocument] = useState<DocumentModel | null>(null);
    const [filteredDocuments, setFilteredDocuments] = useState<DocumentModel[]>([]);

    const { data: documents, isLoading } = useGetActualDocumentsByUserIdQuery({ userId, page: page, pageSize: pageSizeRef.current });
    const [getDocumentByName] = useLazyGetDocumentByNameQuery();

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

    const nameOnChangeAsync = async (e: React.FormEvent<HTMLInputElement>) => {
        try {
            const name = e.currentTarget.value;
            if (name.length === 0) {
                setFilteredDocuments([]);

                return;
            }

            const documents = await getDocumentByName({ userId, name }).unwrap();
            setFilteredDocuments(documents);
        } catch (e) {
            console.log(e);
        }
    }

    if (isLoading || !documents) {
        return (<Loading />);
    }

    if (nameRef.current && nameRef.current.value.length > 0) {
        return (
            <>
                <div className="documents actions">
                    <div>
                        <div className="search-document">
                            <label htmlFor="name">{t("SearchDocument")}</label>
                            <input className="form-control" type="text" id="name" ref={nameRef} onChange={nameOnChangeAsync} />
                        </div>
                    </div>
                    <button className="btn-border-shadow" onClick={() => setIsOpenCreate(true)}>{t("Create")}</button>
                </div>
                {filteredDocuments.length === 0
                    ? <div>{t("NoAnyDocuments")}</div>
                    : <ul className="documents">{filteredDocuments.map((document) => (
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
                {isOpenCreate &&
                    <Create
                        t={t}
                        userId={userId}
                        getTime={getTime}
                        setIsOpenCreate={setIsOpenCreate}
                    />
                }
            </>
        );
    }

    return (
        <>
            <div className="documents actions">
                <div>
                    <div className="search-document">
                        <label htmlFor="name">{t("SearchDocument")}</label>
                        <input className="form-control" type="text" id="name" ref={nameRef} onChange={nameOnChangeAsync} />
                    </div>
                </div>
                <button className="btn-border-shadow" onClick={() => setIsOpenCreate(true)}>{t("Create")}</button>
            </div>
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
            {isOpenCreate &&
                <Create
                    t={t}
                    userId={userId}
                    getTime={getTime}
                    setIsOpenCreate={setIsOpenCreate}
                />
            }
        </>
    );
}

export default Documents;