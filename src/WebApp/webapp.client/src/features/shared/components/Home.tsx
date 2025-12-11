import { useGetAllDocumentsQuery, useLazyGetDocumentByIdQuery } from '@/features/shared/api/Document.api';
import { useRef, useState } from 'react';
import { useTranslation } from 'react-i18next';
import type { DocumentModel } from '../types/DocumentModel';
import Loading from './Loading';

import './Home.scss';

const Home: React.FC = () => {
    const { t } = useTranslation('home');

    const { data: documents, isLoading } = useGetAllDocumentsQuery();
    const [getDocument] = useLazyGetDocumentByIdQuery();

    const documentIdRef = useRef<HTMLInputElement | null>(null);

    const [document, setDocument] = useState<DocumentModel | null>(null);
    
    const getDocumentAsync = async () => {
        try {
            const document = await getDocument(documentIdRef.current ? documentIdRef.current.value : "").unwrap();
            setDocument(document);
        } catch (e) {
            console.log(e);
        }
    }

    if (isLoading) {
        return (<Loading />);
    }

    return (
        <div className="home">
            <div className="home__item">
                <div className="title">{t("Documents")}</div>
                <input className="form-control" type="text" placeholder="Id" ref={documentIdRef} />
                <input type="button" className="btn-border-shadow" value="Get" onClick={getDocumentAsync} />
                <div>{document?.name}</div>
                <ul className="documents">{documents?.map((document) => (
                    <li key={document.id} className="documents__item">
                        <div>{t("Name")}: {document.name}</div>
                        <div>{t("Description")}: {document.description}</div>
                    </li>
                ))}
                </ul>
            </div>
        </div>
    );
}

export default Home;