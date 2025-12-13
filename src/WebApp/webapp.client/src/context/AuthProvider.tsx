import { useLogoutMutation, useLazyRefreshQuery, useLoginMutation } from '@/features/user/api/User.api';
import { updateUser } from '@/features/user/store/UserSlice';
import { AuthContext } from '@/features/user/hooks/useAuth';
import { useState, type ReactNode } from 'react';
import { useDispatch } from 'react-redux';
import { useNavigate } from 'react-router-dom';
import type { LoginModel } from '@/features/user/types/LoginModel';

export const AuthProvider: React.FC<{ children: ReactNode }> = ({ children }) => {
    const dispatch = useDispatch();
    const navigate = useNavigate();

    const [isAuthenticated, setIsAuthenticated] = useState(false);
    const [authInProgress, setAuthInProgress] = useState(false);

    const [getAuth] = useLazyRefreshQuery();
    const [logout] = useLogoutMutation();
    const [login] = useLoginMutation();

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

    const loginAsync = async (loginUser: LoginModel) => {
        try {
            const user = await login(loginUser).unwrap();
            dispatch(updateUser(user));
            setIsAuthenticated(true);

            navigate("/");
        } catch (e) {
            console.log(e);
        }
    }

    const logoutAsync = async () => {
        try {
            await logout().unwrap();

            dispatch(updateUser(null));
            setIsAuthenticated(false);

            navigate("/");
        } catch (e) {
            console.log(e);
        }
    }

    return (
        <AuthContext.Provider value={{ isAuthenticated, authInProgress, checkAuthAsync, loginAsync, logoutAsync }}>
            {children}
        </AuthContext.Provider>
    );
}

export default AuthProvider;