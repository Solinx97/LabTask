import Home from '../features/shared/components/Home';
import Login from '../features/user/components/Login';
import Registration from '../features/user/components/Registration';

type Route = {
    index?: boolean;
    path?: string;
    element: React.ReactNode;
}

const AppRoutes: Route[] = [
    {
        index: true,
        element: <Home />
    },
    {
        path: '/login',
        element: <Login />
    },
    {
        path: '/registration',
        element: <Registration />
    },
];

export default AppRoutes;
