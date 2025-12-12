import { useLazyGetDocumentByNameQuery } from '@/features/document/api/Document.api';
import type { DocumentModel } from '@/features/document/types/DocumentModel';
import { useRef, useState } from 'react';
import Document from './Document';

const Search: React.FC<{ t: (key: string) => string, userId: string }> = ({ t, userId }) => {
    const nameRef = useRef<HTMLInputElement | null>(null);

    const [getDocumentByName] = useLazyGetDocumentByNameQuery();

    const [document, setDocument] = useState<DocumentModel | null>(null);

    const getDocumentByNameAsync = async () => {
        try {
            const document = await getDocumentByName(nameRef.current ? nameRef.current.value : "").unwrap();
            setDocument(document);
        } catch (e) {
            console.log(e);
        }
    }

    return (
        <div className="search-document">
            <div className="title">{t("SearchDocument")}</div>
            <input className="form-control" type="text" placeholder={t("Name")} ref={nameRef} />
            <button type="button" className="btn-border-shadow" onClick={getDocumentByNameAsync}>{t("Get")}</button>
            {document &&
                <Document
                    t={t}
                    userId={userId}
                    document={document}
                />
            }
        </div>
    );
}

export default Search;