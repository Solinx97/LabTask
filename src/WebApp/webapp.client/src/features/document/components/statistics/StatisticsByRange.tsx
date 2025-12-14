import { useLazyGetDocumentStatisticsByRangeQuery } from '@/features/document/api/Document.api';
import { useRef, useState } from 'react';
import type { StatisticModel } from '../../types/StatisticModel';

interface Props {
    t: (key: string) => string;
    userId: string;
    getTime: (dateAsString?: string) => string;
}

const StatisticsByRange: React.FC<Props> = ({ t, userId, getTime }) => {
    const startedAtRef = useRef<HTMLInputElement | null>(null);
    const finishedAtRef = useRef<HTMLInputElement | null>(null);

    const [getStatistics] = useLazyGetDocumentStatisticsByRangeQuery();

    const [statistics, setStatistics] = useState<StatisticModel[]>([]);

    const getStatisticsAsync = async () => {
        try {
            if (!startedAtRef.current || !finishedAtRef.current) {
                return;
            }

            const agrs = {
                userId: userId,
                startedAt: startedAtRef.current.value,
                finishedAt: finishedAtRef.current.value
            };

            var statistiscs = await getStatistics(agrs).unwrap();
            setStatistics(statistiscs);
        } catch (e) {
            console.log(e);
        }
    }

    return (
        <div className="statistics">
            <div>{t("SelectRange")}</div>
            <div className="mb-3">
                <label htmlFor="startedAt">{t("StartedAt")}</label>
                <input className="form-control" type="datetime-local" id="startedAt" defaultValue={getTime()} ref={startedAtRef} />
            </div>
            <div className="mb-3">
                <label htmlFor="finishedAt">{t("FinishedAt")}</label>
                <input className="form-control" type="datetime-local" id="finishedAt" defaultValue={getTime()} ref={finishedAtRef} />
            </div>
            <button className="btn-border-shadow" onClick={getStatisticsAsync}>{t("Get")}</button>
            {statistics.length === 0
                ? <div>{t("NoAnyStatistics")}</div>
                : <ul className="statistics__content">{statistics.map((statistic, index) => (
                    <li key={index}>
                        <p>{t("Year")}: {statistic.year}</p>
                        <p>{t("CreatedAtDocumentCount")}: {statistic.createdAtCount}</p>
                        <p>{t("UpdatedAtDocumentCount")}: {statistic.updatedAtCount}</p>
                        <p>{t("ExpiredAtDocumentCount")}: {statistic.expiredAtCount}</p>
                    </li>
                ))}
                </ul>
            }
        </div>
    );
}

export default StatisticsByRange;