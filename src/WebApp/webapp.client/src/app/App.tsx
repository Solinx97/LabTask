import Layout from '@/features/shared/components/Layout';
import { Route, Routes } from 'react-router-dom';
import AppRoutes from './Routes';

import './App.css';

const App: React.FC = () => {
    return (
        <Layout>
            <Routes>
                {AppRoutes.map((route, index) => {
                    const { element, ...rest } = route;
                    return <Route key={index} {...rest} element={element} />;
                })}
            </Routes>
        </Layout>
    );
}

export default App;