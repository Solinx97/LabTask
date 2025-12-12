import type { CommentModel } from '../types/CommentModel';
import { DocumentApi } from './Dcoument.api';

export const CommentApi = DocumentApi.injectEndpoints({
    endpoints: builder => ({
        createComment: builder.mutation<void, CommentModel>({
            query: Comment => ({
                body: Comment,
                url: '/Comment',
                method: 'POST'
            }),
            invalidatesTags: (result, error, { id }) => [
                { type: "Comment", id },
            ]
        }),
        updateComment: builder.mutation<void, CommentModel>({
            query: Comment => ({
                body: Comment,
                url: `/Comment/${Comment.id}`,
                method: 'PATCH'
            }),
            invalidatesTags: (result, error, { id }) => [
                { type: "Comment", id },
            ]
        }),
        deleteComment: builder.mutation<void, string>({
            query: id => ({
                url: `/Comment/${id}`,
                method: 'DELETE'
            }),
            invalidatesTags: (_result, _error, id) => [{ type: 'Comment', id }],
        }),
        getAllComments: builder.query<CommentModel[], void>({
            query: () => "/Comment",
            providesTags: result =>
                result
                    ? [
                        ...result.map(docuemnt => ({ type: 'Comment' as const, id: docuemnt.id })),
                        { type: 'Comment', id: 'LIST' },
                    ]
                    : [{ type: 'Comment', id: 'LIST' }],
        }),
        getCommentById: builder.query<CommentModel, string>({
            query: id => `/Comment/${id}`,
            providesTags: result => result ? [{ type: 'Comment', id: result.id }] : [],
        }),
        getCommentsByUserId: builder.query<CommentModel[], string>({
            query: userId => `/Comment/getByUserId/${userId}`,
            providesTags: result =>
                result
                    ? [
                        ...result.map(docuemnt => ({ type: 'Comment' as const, id: docuemnt.id })),
                        { type: 'Comment', id: 'LIST' },
                    ]
                    : [{ type: 'Comment', id: 'LIST' }],
        }),
    })
})

export const {
    useCreateCommentMutation,
    useUpdateCommentMutation,
    useDeleteCommentMutation,
    useGetAllCommentsQuery,
    useLazyGetCommentByIdQuery,
    useGetCommentsByUserIdQuery,
} = CommentApi;