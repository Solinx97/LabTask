import type { RootState } from '@/app/Store';
import React from 'react';
import { useTranslation } from 'react-i18next';
import { useSelector } from 'react-redux';
import { Link, useNavigate } from 'react-router-dom';
import { Container, Navbar, NavbarBrand } from 'reactstrap';
import LanguageSelector from './LanguageSelector';
import { useLogoutMutation } from '@/features/user/api/User.api';
import { useDispatch } from 'react-redux';
import { updateUser } from '@/features/user/store/UserSlice';

import './NavMenu.scss';

const NavMenu: React.FC = () => {
    const { t } = useTranslation('translate');

    const dispatch = useDispatch();
    const navigate = useNavigate();

    const user = useSelector((state: RootState) => state.user.value);

    const [logout] = useLogoutMutation();

    const logoutAsync = async () => {
        try {
            await logout().unwrap();
            dispatch(updateUser(null));

            navigate("/");
        } catch (e) {
            console.log(e);
        }
    }

    return (
        <header>
            <Navbar className="navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow mb-3" light>
                <Container>
                    <div className="brand-container">
                        <LanguageSelector />
                        <div className="brand">
                            <NavbarBrand
                                tag={Link}
                                to="/"
                            >
                                Lab
                            </NavbarBrand>
                        </div>
                    </div>
                    {user
                        ? <div className="authorized">
                            <div className="username">{user?.email}</div>
                            <div className="authorized__logout" onClick={logoutAsync}>{t("Logout")}</div>
                        </div>
                        : <div className="authorization">
                            <div className="authorization__login" onClick={() => navigate("/login")}>{t("Login")}</div>
                            <div className="authorization__registration" onClick={() => navigate("/registration")}>{t("Registration")}</div>
                        </div>
                    }
                </Container>
            </Navbar>
        </header>
    );
}

export default NavMenu;
