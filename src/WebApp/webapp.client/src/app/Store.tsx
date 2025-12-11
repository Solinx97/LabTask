import { combineReducers, configureStore } from '@reduxjs/toolkit';
import { DocumentApi } from '../features/shared/api/Document.api';
import { UserApi } from '../features/user/api/User.api';
import userReducer from '../features/user/store/UserSlice';

const reducers = combineReducers({
    user: userReducer,
    [UserApi.reducerPath]: UserApi.reducer,
    [DocumentApi.reducerPath]: DocumentApi.reducer,
});

const Store = configureStore({
    reducer: reducers,
    middleware: (getDefaultMiddleware) =>
        getDefaultMiddleware()
            .concat(UserApi.middleware)
            .concat(DocumentApi.middleware)
});

export type RootState = ReturnType<typeof Store.getState>;

export default Store;