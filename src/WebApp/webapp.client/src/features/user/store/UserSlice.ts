import { createSlice } from '@reduxjs/toolkit';
import type { UserModel } from '../types/UserModel';

type UserSlice = {
    value: UserModel | null;
}

const initialState: UserSlice = {
    value: null,
}

export const userSlice = createSlice({
    name: 'user',
    initialState,
    reducers: {
        updateUser: (state, action) => {
            state.value = action.payload
        },
    },
})

export const { updateUser } = userSlice.actions

export default userSlice.reducer