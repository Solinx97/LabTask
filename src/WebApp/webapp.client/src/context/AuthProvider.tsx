import { useLazyRefreshQuery } from '@/features/user/api/User.api';
import { updateUser } from '@/features/user/store/UserSlice';
import { AuthContext } from '@/features/user/hooks/useAuth';
import { useState, type ReactNode } from 'react';
import { useDispatch } from 'react-redux';

export const AuthProvider: React.FC<{ children: ReactNode }> = ({ children }) => {
    const dispatch = useDispatch();

    const [isAuthenticated, setIsAuthenticated] = useState(false);
    const [authInProgress, setAuthInProgress] = useState(false);

    const [getAuth] = useLazyRefreshQuery();

    const checkAuthAsync = async () => {
        try {
            setAuthInProgress(true);

            const response = await getAuth();
            if (response.data !== undefined) {
                const user = response.data;

                dispatch(updateUser(user));

                setIsAuthenticated(true);
            }

            setAuthInProgress(false);
        } catch (e) {
            dispatch(updateUser(null));

            setAuthInProgress(false);
        }
    }

    return (
        <AuthContext.Provider value={{ isAuthenticated, authInProgress, checkAuthAsync }}>
            {children}
        </AuthContext.Provider>
    );
}

export default AuthProvider;