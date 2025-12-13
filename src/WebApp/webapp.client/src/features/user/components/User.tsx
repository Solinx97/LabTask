import { useGetUserByIdQuery } from '@/features/user/api/User.api';

const User: React.FC<{ userId: string }> = ({ userId }) => {
    const { data: user, isLoading } = useGetUserByIdQuery(userId);

    if (isLoading || !user) {
        return (<></>);
    }

    return (
        <div>{user.email}</div>
    );
}

export default User;