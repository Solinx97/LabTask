import type { RootState } from '@/app/Store';
import { useState } from 'react';
import { useTranslation } from 'react-i18next';
import Create from '@/features/document/components/Create';
import Documents from '@/features/document/components/Documents';
import Search from '@/features/document/components/Search';
import History from '@/features/document/components/History';
import { useSelector } from 'react-redux';

import './Home.scss';
import Manage from '@/features/document/components/Manage';

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

    const getTime = (dateAsString?: string) => {
        const date = dateAsString ? new Date(dateAsString) : new Date();

        const internationalMonth = date.getMonth() + 1;
        const month = internationalMonth < 10 ? `0${internationalMonth}` : internationalMonth;
        const day = date.getDay() < 10 ? `0${date.getDay()}` : date.getDay();
        const hours = date.getHours() < 10 ? `0${date.getHours()}` : date.getHours();
        const minutes = date.getMinutes() < 10 ? `0${date.getMinutes()}` : date.getMinutes();

        const currentDate = `${date.getFullYear()}-${month}-${day} ${hours}:${minutes}:00`;

        return currentDate;
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
                        getTime={getTime}
                    />
                }
                {section === 1 &&
                    <Documents
                        t={t}
                        userId={user?.id ?? ""}
                        getTime={getTime}
                    />
                }
                {section === 2 &&
                    <Search
                        t={t}
                        userId={user?.id ?? ""}
                    />
                }
                {section === 3 &&
                    <History
                        t={t}
                        userId={user?.id ?? ""}
                        getTime={getTime}
                    />
                }
                {section === 4 &&
                    <Manage
                        t={t}
                        userId={user?.id ?? ""}
                        getTime={getTime}
                    />
                }
            </div>
        </div>
    );
}

export default Home;