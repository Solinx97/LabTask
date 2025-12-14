import type { RootState } from '@/app/Store';
import { useCallback, useState } from 'react';
import { useTranslation } from 'react-i18next';
import Documents from '@/features/document/components/Documents';
import History from '@/features/document/components/History';
import { useSelector } from 'react-redux';

import './Home.scss';
import Manage from '@/features/document/components/Manage';
import Statistics from '@/features/document/components/Statistics';

const Home: React.FC = () => {
    const { t } = useTranslation('home');

    const user = useSelector((state: RootState) => state.user.value);

    const [section, setSection] = useState(-1);

    const getMenu = () => {
        const menu = [t("MyDocuments"), t("History"), t("Manage"), t("Statistics")];

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

    const getTime = useCallback((dateAsString?: string) => {
        const date = dateAsString ? new Date(dateAsString) : new Date();

        const month = String(date.getMonth() + 1).padStart(2, "0");
        const day = String(date.getDate()).padStart(2, "0");
        const hours = String(date.getHours()).padStart(2, "0");
        const minutes = String(date.getMinutes()).padStart(2, "0");

        return `${date.getFullYear()}-${month}-${day} ${hours}:${minutes}:00`;
    }, []);

    return (
        <div className="home">
            <div className="home__item">
                <div className="manage">
                    {getMenu()}
                </div>
                {section === 0 &&
                    <Documents
                        t={t}
                        userId={user?.id ?? ""}
                        getTime={getTime}
                    />
                }
                {section === 1 &&
                    <History
                        t={t}
                        userId={user?.id ?? ""}
                        getTime={getTime}
                    />
                }
                {section === 2 &&
                    <Manage
                        t={t}
                        userId={user?.id ?? ""}
                        getTime={getTime}
                    />
                }
                {section === 3 &&
                    <Statistics
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