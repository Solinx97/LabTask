import { useLoginMutation } from '@/features/user/api/User.api';
import { updateUser } from '@/features/user/store/UserSlice';
import { useRef } from 'react';
import { useDispatch } from 'react-redux';
import { useNavigate } from 'react-router-dom';
import type { LoginModel } from '../types/LoginModel';
import { useTranslation } from 'react-i18next';

const Login = () => {
    const { t } = useTranslation('account');

    const dispatch = useDispatch();
    const navigate = useNavigate();

    const emailRef = useRef<HTMLInputElement | null>(null);
    const passwordRef = useRef<HTMLInputElement | null>(null);

    const [login] = useLoginMutation();

    const loginAsync = async (e: React.FormEvent) => {
        e.preventDefault();

        try {
            const userLogin: LoginModel = {
                email: emailRef.current ? emailRef.current.value : "",
                password: passwordRef.current ? passwordRef.current.value : ""
            };

            const user = await login(userLogin).unwrap();
            dispatch(updateUser(user));

            navigate("/");
        } catch (e) {
            console.log(e);
        }
    }

    return (
        <form className="login" onSubmit={loginAsync}>
            <div className="mb-3">
                <input className="form-control" type="email" placeholder="Email" ref={emailRef} required />
            </div>
            <div className="mb-3">
                <input className="form-control" type="password" placeholder={t("Password")} ref={passwordRef} required />
            </div>
            <div className="actions">
                <input type="submit" className="btn-border-shadow finish-login" value={t("Login")} />
            </div>
        </form>
    );
}

export default Login;