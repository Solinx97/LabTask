import { useGetDocumentsByUserIdQuery, useDeleteDocumentMutation, useUpdateDocumentMutation, useLazyGetDocumentByIdQuery } from '@/features/document/api/Document.api';
import Loading from '@/features/shared/components/Loading';
import type { UserModel } from '@/features/user/types/UserModel';
import { useRef, useState } from 'react';
import type { DocumentModel } from '../types/DocumentModel';

const Documents:React.FC<{ t: (key: string) => string, user: UserModel | null}> = ({ t, user }) => {
    const { data: myDocuments, isLoading } = useGetDocumentsByUserIdQuery(user?.id ?? "");
    const [getDocumentById] = useLazyGetDocumentByIdQuery();
    const [deleteDocument] = useDeleteDocumentMutation();
    const [updateDocument] = useUpdateDocumentMutation();

    const nameRef = useRef<HTMLInputElement | null>(null);
    const descriptiondRef = useRef<HTMLTextAreaElement | null>(null);
    
    const [isEditMode, setIsEditMode] = useState(false);

    const deleteAsync = async (id: string) => {
        try {
            await deleteDocument(id).unwrap();
        } catch (e) {
            console.log(e);
        }
    }

    const updateAsync = async (selectedDocument: DocumentModel) => {
        try {
            const document: DocumentModel = {
                id: selectedDocument.id,
                name: nameRef.current ? nameRef.current.value : "",
                description: descriptiondRef.current ? descriptiondRef.current.value : "",
                expireAt: selectedDocument.expireAt,
                userId: selectedDocument.userId
            };
            
            await updateDocument(document).unwrap();

            setIsEditMode(false);
        } catch (e) {
            console.log(e);
        }
    }

    const getDocumentByIdAsync = async (id: string) => {
        try {
            const document = await getDocumentById(id).unwrap();
        } catch (e) {
            console.log(e);
        }
    }

    if (isLoading) {
        return (<Loading />);
    }

    return (
        <ul className="documents">{myDocuments?.map((document) => (
            <li key={document.id} className="documents__item">
                {isEditMode
                    ?
                        <>
                            <input type="text" defaultValue={document.name} ref={nameRef} />
                            <textarea rows={6} defaultValue={document.description} ref={descriptiondRef} />
                            <button className="btn-border-shadow" onClick={async () => await updateAsync(document)}>Save</button>
                            <button className="btn-border-shadow" onClick={() => setIsEditMode(false)}>Cancel</button>
                        </>
                    :
                        <>
                            <div>{t("Name")}: {document.name}</div>
                            <div>{t("Description")}: {document.description}</div>
                            <button className="btn-border-shadow" onClick={async () => await deleteAsync(document.id)}>Delete</button>
                            <button className="btn-border-shadow" onClick={() => setIsEditMode(true)}>Update</button>
                        </>
                }
            </li>
        ))}
        </ul>
    );
}

export default Documents;