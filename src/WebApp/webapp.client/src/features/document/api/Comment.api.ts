import type { CommentModel } from '../types/CommentModel';
import { DocumentApi } from './Document.api';

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
        deleteComment: builder.mutation<void, { documentId: string, commentId: string }>({
            query: ({ documentId, commentId }) => ({
                url: `/Comment/documents/${documentId}/comments/${commentId}`,
                method: 'DELETE'
            }),
            invalidatesTags: (result, error, { commentId }) => [
                { type: "Comment", commentId },
            ]
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
        getCommentsByDocumentId: builder.query<CommentModel[], string>({
            query: documentId => `/Comment/getByDocumentId/${documentId}`,
            providesTags: result =>
                result
                    ? [
                        ...result.map(comment => ({ type: 'Comment' as const, id: comment.id })),
                        { type: 'Comment', id: 'LIST' },
                    ]
                    : [{ type: 'Comment', id: 'LIST' }],
        }),
        getCommentsByUserId: builder.query<CommentModel[], string>({
            query: userId => `/Comment/getByUserId/${userId}`,
            providesTags: result =>
                result
                    ? [
                        ...result.map(comment => ({ type: 'Comment' as const, id: comment.id })),
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
    useGetCommentsByDocumentIdQuery,
    useGetCommentsByUserIdQuery,
} = CommentApi;