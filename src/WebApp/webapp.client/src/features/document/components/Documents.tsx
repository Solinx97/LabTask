import { useGetDocumentsByUserIdQuery } from '@/features/document/api/Document.api';
import Loading from '@/features/shared/components/Loading';
import Document from './Document';

const Documents:React.FC<{ t: (key: string) => string, userId: string}> = ({ t, userId }) => {
    const { data: myDocuments, isLoading } = useGetDocumentsByUserIdQuery(userId);

    if (isLoading) {
        return (<Loading />);
    }

    return (
        <>
        {myDocuments?.length === 0
            ? <div>{t("NoAnyDocuments")}</div>
            : <ul className="documents">{myDocuments?.map((document) => (
                <li key={document.id} className="container">
                    <Document
                        t={t}
                        userId={userId}
                        document={document}
                    />
                </li>
            ))}
            </ul>
        }
        </>
    );
}

export default Documents;