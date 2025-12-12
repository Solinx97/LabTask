import type { RootState } from '@/app/Store';
import { useState } from 'react';
import { useTranslation } from 'react-i18next';
import Create from '@/features/document/components/Create';
import Documents from '@/features/document/components/Documents';
import Search from '@/features/document/components/Search';
import { useSelector } from 'react-redux';

import './Home.scss';

const Home: React.FC = () => {
    const { t } = useTranslation('home');

    const user = useSelector((state: RootState) => state.user.value);

    const [section, setSection] = useState(-1);

    const getMenu = () => {
        const menu = [t("CreateDocument"), t("MyDocuments"), t("SearchDocument"), t("History"), t("Manage")];

        if (user) {
            return (
                menu.map((item, index) => (
                    <button key={index} className={`btn-border-shadow ${section === index ? 'green' : ''}`} onClick={() => setSection(index)}>{item}</button>
                ))
            );
        }
        else {
            return (
                menu.map((item, index) => (
                    <button key={index} className={`btn-border-shadow ${section === index ? 'green' : ''}`} disabled>{item}</button>
                ))
            );
        }
    }

    return (
        <div className="home">
            <div className="home__item">
                <div className="manage">
                    {getMenu()}
                </div>
                {section === 0 &&
                    <Create
                        t={t}
                        userId={user?.id ?? ""}
                    />
                }
                {section === 1 &&
                    <Documents
                        t={t}
                        userId={user?.id ?? ""}
                    />
                }
                {section === 2 &&
                    <Search
                        t={t}
                        userId={user?.id ?? ""}
                    />
                }
            </div>
        </div>
    );
}

export default Home;