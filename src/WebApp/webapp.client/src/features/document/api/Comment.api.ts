import type { CommentModel } from '../types/CommentModel';
import { DocumentApi } from './Document.api';

export const CommentApi = DocumentApi.injectEndpoints({
    endpoints: builder => ({
        createComment: builder.mutation<void, CommentModel>({
            query: comment => ({
                body: comment,
                url: '/Comment',
                method: 'POST'
            }),
            invalidatesTags: (result, error, { id }) => [
                { type: "Comment", id },
            ]
        }),
        updateComment: builder.mutation<void, CommentModel>({
            query: comment => ({
                body: comment,
                url: `/Comment/${comment.id}`,
                method: 'PATCH'
            }),
            async onQueryStarted(comment, { dispatch, queryFulfilled }) {
                const patchResult = dispatch(
                    CommentApi.util.updateQueryData(
                        'getCommentsByDocumentId',
                        { documentId: comment.documentId },
                        draft => {
                            const com = draft.find(c => c.id === comment.id);
                            if (com) {
                                Object.assign(com, comment);
                            }
                        }
                    )
                );
                try {
                    await queryFulfilled;
                } catch {
                    patchResult.undo();
                }
            }
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
        getCommentsByDocumentId: builder.query<CommentModel[], { documentId: string, page: number, pageSize: number }>({
            query: ({ documentId, page, pageSize }) => `/Comment/getByDocumentId/${documentId}?page=${page}&pageSize=${pageSize}`,
            serializeQueryArgs: ({ endpointName, queryArgs }) => `${endpointName}-${queryArgs.documentId}`,
            merge: (currentCache, newItems) => {
                newItems.forEach(item => {
                    const index = currentCache.findIndex(d => d.id === item.id);
                    if (index === -1) {
                        currentCache.push(item);
                    } else {
                        currentCache[index] = item;
                    }
                });
            },
            forceRefetch: ({ currentArg, previousArg }) => currentArg?.page !== previousArg?.page,
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