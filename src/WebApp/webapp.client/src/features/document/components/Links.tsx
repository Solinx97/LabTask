import { useUsersQuery } from '@/features/user/api/User.api';
import { useGetLinksByOwnerIdQuery, useDeleteLinkMutation } from '@/features/document/api/Link.api';
import Loading from '@/features/shared/components/Loading';
import { useRef, useState, type SetStateAction } from 'react';
import type { DocumentModel } from '../types/DocumentModel';
import User from '@/features/user/components/User';
import { faTrash, faPlus } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import CreateLink from './CreateLink';

interface Props {
    t: (key: string) => string;
    userId: string;
    document: DocumentModel | null;
    setIsOpenLinks: (value: SetStateAction<boolean>) => void;
    getTime: (dateAsString?: string) => string;
}

const Links: React.FC<Props> = ({ t, userId, document, setIsOpenLinks, getTime }) => {
    const pageSizeRef = useRef<number>(5);

    const [page, setPage] = useState(0);
    const [isCreateLinkOpen, setIsCreateLinkOpen] = useState(false);

    const [deleteLink] = useDeleteLinkMutation();

    const { data: users, isLoading } = useUsersQuery();
    const { data: ownLinks, isLoading: linkIsLoading } = useGetLinksByOwnerIdQuery({ userId, page: page, pageSize: pageSizeRef.current });

    const deleteAsync = async (id: string) => {
        try {
            await deleteLink({ id, userId }).unwrap();
        } catch (e) {
            console.log(e);
        }
    }

    if (isLoading || !users || linkIsLoading || !ownLinks) {
        return (<Loading />);
    }

    return (
        <div className="link modal-window">
            <div className="actions">
                <div>{t("Links")}</div>
                <FontAwesomeIcon
                    icon={faPlus}
                    onClick={() => setIsCreateLinkOpen(prev => !prev)}
                />
            </div>
            {isCreateLinkOpen &&
                <CreateLink
                    t={t}
                    userId={userId}
                    document={document}
                    setIsCreateLinkOpen={setIsCreateLinkOpen}
                    getTime={getTime}
                />
            }
            <div className="exist-links">
                <div>{t("CreatedLinks")}</div>
                {ownLinks.length === 0
                    ? <div>{t("NoAnyLinks")}</div>
                    : <ul className="container">{ownLinks.map(link => (
                        <li key={link.id} className="exist-links__item actions">
                            <User
                                userId={link.toUserId}
                            />
                            <FontAwesomeIcon
                                className="danger"
                                icon={faTrash}
                                onClick={async () => await deleteAsync(link.id)}
                            />
                        </li>
                    ))}
                        <li className="exist-links__load-more" onClick={() => setPage(prev => prev + 1)}>{t("LoadMore")}</li>
                    </ul>
                }
            </div>
            <button className="btn-border-shadow" onClick={() => setIsOpenLinks(false)}>{t("Cancel")}</button>
        </div>
    )
}

export default Links;