import { useUsersQuery } from '@/features/user/api/User.api';
import { useCreateLinkMutation, useGetLinksByOwnerIdQuery } from '@/features/document/api/Link.api';
import Loading from '@/features/shared/components/Loading';
import { useRef, useState, type SetStateAction } from 'react';
import type { LinkModel } from '../types/LinkModel';
import type { DocumentModel } from '../types/DocumentModel';

interface Props {
    t: (key: string) => string;
    userId: string;
    document: DocumentModel | null;
    setIsCreateLinkOpen: (value: SetStateAction<boolean>) => void;
    getTime: (dateAsString?: string) => string;
}

const CreateLink: React.FC<Props> = ({ t, userId, document, setIsCreateLinkOpen, getTime }) => {
    const pageSizeRef = useRef<number>(5);
    const userRef = useRef<HTMLSelectElement | null>(null);
    const expiresAtRef = useRef<HTMLInputElement | null>(null);

    const [page, setPage] = useState(0);

    const [createLink] = useCreateLinkMutation();

    const { data: users, isLoading } = useUsersQuery();
    const { data: ownLinks, isLoading: linkIsLoading } = useGetLinksByOwnerIdQuery({ userId, page: page, pageSize: pageSizeRef.current });

    const createAsync = async (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();

        try {
            if (!userRef.current || !expiresAtRef.current) {
                return;
            }

            const link: LinkModel = {
                id: crypto.randomUUID(),
                uri: crypto.randomUUID(),
                documentId: document?.id ?? "",
                ownerId: userId,
                toUserId: userRef.current ? userRef.current.value : "",
                expireAt: expiresAtRef.current ? expiresAtRef.current.value : "",
            };
            await createLink(link).unwrap();

            userRef.current.value = "";
        } catch (e) {
            console.log(e);
        }
    }

    if (isLoading || !users || linkIsLoading || !ownLinks) {
        return (<Loading />);
    }

    return (
        <form className="link__create" onSubmit={createAsync}>
            <div className="link__title">{t("CreateLink")}</div>
            <div className="form-group">
                <label htmlFor="document-name">{t("DocumentName")}:</label>
                <div className="col-sm-10">
                    <input type="text" className="form-control-plaintext" id="document-name" value={document?.name} title={document?.name} readOnly />
                </div>
            </div>
            <div className="form-group">
                <label htmlFor="user">{t("CreateFor")}:</label>
                <select className="form-control" name="user" id="user" ref={userRef}>
                    {users.map(user => (
                        <option key={user.id} value={user.id}>{user.email}</option>
                    ))}
                </select>
            </div>
            <div className="form-group">
                <label htmlFor="expires">{t("ExpiresAt")}:</label>
                <input className="form-control" type="datetime-local" defaultValue={getTime()} id="expires" name="expires" ref={expiresAtRef} required />
            </div>
            <div className="actions">
                <input type="submit" className="btn-border-shadow" value={t("Create")} />
                <input type="button" className="btn-border-shadow orange" value={t("Cancel")} onClick={() => setIsCreateLinkOpen(false)} />
            </div>
        </form>
    )
}

export default CreateLink;