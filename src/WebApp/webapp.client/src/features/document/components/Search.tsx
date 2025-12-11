import { useLazyGetDocumentByIdQuery } from '@/features/document/api/Document.api';
import type { DocumentModel } from '@/features/document/types/DocumentModel';
import { useRef, useState } from 'react';

const Search:React.FC<{ t: (key: string) => string}> = ({ t }) => {
    const documentIdRef = useRef<HTMLInputElement | null>(null);
    
    const [getDocument] = useLazyGetDocumentByIdQuery();

    const [document, setDocument] = useState<DocumentModel | null>(null);

    const getDocumentByidAsync = async () => {
        try {
            const document = await getDocument(documentIdRef.current ? documentIdRef.current.value : "").unwrap();
            setDocument(document);
        } catch (e) {
            console.log(e);
        }
    }

    return (
        <div>
            <div className="title">{t("Search")}</div>
            <input className="form-control" type="text" placeholder="Id" ref={documentIdRef} />
            <button type="button" className="btn-border-shadow" onClick={getDocumentByidAsync}>{t("Get")}</button>
            <div>{document?.name}</div>
        </div>
    );
}

export default Search;