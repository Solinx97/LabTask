import { useGetDocumentStatisticsByYearQuery } from '@/features/document/api/Document.api';

const StatisticsByYear: React.FC<{ t: (key: string) => string, userId: string }> = ({ t, userId }) => {
    const { data: statistics, isLoading } = useGetDocumentStatisticsByYearQuery(userId);

    if (isLoading || !statistics) {
        return (<></>);
    }

    return (
        <div>
            <ul>{statistics.map((statistic, index) => (
                <li key={index}>
                    <p>{t("Year")}: {statistic.year}</p>
                    <p>{t("CreatedAtDocumentCount")}: {statistic.createdAtCount}</p>
                    <p>{t("UpdatedAtDocumentCount")}: {statistic.updatedAtCount}</p>
                    <p>{t("ExpiredAtDocumentCount")}: {statistic.expiredAtCount}</p>
                </li>
            ))}
            </ul>
        </div>
    );
}

export default StatisticsByYear;