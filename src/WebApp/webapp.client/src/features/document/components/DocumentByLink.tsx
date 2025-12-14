import { useGetDocumentByIdQuery } from '@/features/document/api/Document.api';
import Document from './Document';

interface Props {
    t: (key: string) => string;
    id: string;
    userId: string;
    getTime: (dateAsString?: string) => string;
}

const DocumentByLink: React.FC<Props> = ({ t, id, userId, getTime }) => {
    const { data: document, isLoading } = useGetDocumentByIdQuery(id);

    if (isLoading || !document) {
        return (<></>);
    }

    return (
        <Document
            t={t}
            userId={userId}
            document={document}
            getTime={getTime}
            deleteAllow={false}
            linksAllow={false}
        />
    );
}

export default DocumentByLink;