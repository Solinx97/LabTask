import type { RootState } from '@/app/Store';
import React, { useEffect } from 'react';
import { useTranslation } from 'react-i18next';
import { useSelector } from 'react-redux';
import { Link, useNavigate } from 'react-router-dom';
import { Container, Navbar, NavbarBrand } from 'reactstrap';
import LanguageSelector from './LanguageSelector';
import { useAuth } from '@/features/user/hooks/useAuth';

import './NavMenu.scss';

const NavMenu: React.FC = () => {
    const { t } = useTranslation('translate');

    const navigate = useNavigate();

    const auth = useAuth();

    const user = useSelector((state: RootState) => state.user.value);

    useEffect(() => {
        const checkAuth = async () => {
            try {
                await auth?.checkAuthAsync();
            } catch (e) {
                console.log(e)
            }
        }

        checkAuth();
    }, []);

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
                    {auth?.authInProgress
                        ? <div>{t("LoginInProgress")}</div>
                        : <div className="main-elements">
                            {auth?.isAuthenticated
                                ? <div className="authorized">
                                    <div className="username">{user?.email}</div>
                                    <div className="authorized__logout" onClick={auth?.logoutAsync}>{t("Logout")}</div>
                                </div>
                                : <div className="authorization">
                                    <div className="authorization__login" onClick={() => navigate("/login")}>{t("Login")}</div>
                                    <div className="authorization__registration" onClick={() => navigate("/registration")}>{t("Registration")}</div>
                                </div>
                            }
                        </div>
                    }
                </Container>
            </Navbar>
        </header>
    );
}

export default NavMenu;
