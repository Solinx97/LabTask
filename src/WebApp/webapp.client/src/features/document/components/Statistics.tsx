import Accordion from 'react-bootstrap/Accordion';
import StatisticsByYear from './statistics/StatisticsByYear';
import StatisticsByRange from './statistics/StatisticsByRange';
import { useState } from 'react';
import type { AccordionEventKey } from 'react-bootstrap/esm/AccordionContext';

interface Props {
    t: (key: string) => string;
    userId: string;
    getTime: (dateAsString?: string) => string;
}

const Statistics: React.FC<Props> = ({ t, userId, getTime }) => {
    const [item, setItem] = useState(-1);

    const onSelectHandler = (eventKey: AccordionEventKey) => {
        const number = eventKey?.toString() ?? "-1";
        setItem(parseInt(number));
    }

    return (
        <Accordion onSelect={onSelectHandler}>
            <Accordion.Item eventKey="0">
                <Accordion.Header>{t("StatisticsByYers")}</Accordion.Header>
                <Accordion.Body>
                    {item === 0 &&
                        <StatisticsByYear
                            t={t}
                            userId={userId}
                        />
                    }
                </Accordion.Body>
            </Accordion.Item>
            <Accordion.Item eventKey="1">
                <Accordion.Header>{t("StatisticsByRange")}</Accordion.Header>
                <Accordion.Body>
                    {item === 1 &&
                        <StatisticsByRange
                            t={t}
                            userId={userId}
                            getTime={getTime}
                        />
                    }
                </Accordion.Body>
            </Accordion.Item>
        </Accordion>
    );
}

export default Statistics;