import { useRegistrationMutation } from '@/features/user/api/User.api';
import { useRef } from 'react';
import { useNavigate } from 'react-router-dom';
import { useTranslation } from 'react-i18next';
import type { RegistrationModel } from '../types/RegistrationModel';

import './User.scss';

const Registration = () => {
    const { t } = useTranslation('account');

    const navigate = useNavigate();

    const emailRef = useRef<HTMLInputElement | null>(null);
    const passwordRef = useRef<HTMLInputElement | null>(null);
    const confirmPasswordRef = useRef<HTMLInputElement | null>(null);

    const [registration] = useRegistrationMutation();

    const registrationAsync = async (e: React.FormEvent) => {
        e.preventDefault(); 

        try {
            if (passwordRef.current?.value !== confirmPasswordRef.current?.value) {
                return;
            }
            
            const userRegistration: RegistrationModel = {
                email: emailRef.current ? emailRef.current.value : "",
                password: passwordRef.current ? passwordRef.current.value : ""
            };

            await registration(userRegistration).unwrap();

            navigate("/");
        } catch (e) {
            console.log(e);
        }
    }

    return (
        <form className="registration" onSubmit={registrationAsync}>
            <div className="mb-3">
                <input className="form-control" type="email" placeholder="Email" ref={emailRef} required />
            </div>
            <div className="mb-3">
                <input className="form-control" type="password" placeholder={t("Password")} ref={passwordRef} required />
            </div>
            <div className="mb-3">
                <input className="form-control" type="password" placeholder={t("ConfirmPassword")} ref={confirmPasswordRef} required />
            </div>
            <div className="actions">
                <input type="submit" className="btn-border-shadow finish-login" value={t("Registration")} />
            </div>
        </form>
    );
}

export default Registration;