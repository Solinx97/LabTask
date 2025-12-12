import Layout from '@/features/shared/components/Layout';
import { Route, Routes } from 'react-router-dom';
import AppRoutes from './Routes';
import AuthProvider from '@/context/AuthProvider';

import './App.css';

const App: React.FC = () => {
    return (
        <AuthProvider>
            <Layout>
                <Routes>
                    {AppRoutes.map((route, index) => {
                        const { element, ...rest } = route;
                        return <Route key={index} {...rest} element={element} />;
                    })}
                </Routes>
            </Layout>
        </AuthProvider>
    );
}

export default App;