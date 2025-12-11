import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';
import type { DocumentModel } from '../types/DocumentModel';

const apiURL = '/api/v1';

export const DocumentApi = createApi({
    reducerPath: 'documentApi',
    tagTypes: [
        'Document',
    ],
    baseQuery: fetchBaseQuery({
        baseUrl: apiURL
    }),
    endpoints: builder => ({
        createDocument: builder.mutation<DocumentModel, DocumentModel>({
            query: document => ({
                body: document,
                url: '/Document',
                method: 'POST'
            }),
            invalidatesTags: result => result ? [{ type: 'Document', id: result.id }] : [],
        }),
        getAllDocuments: builder.query<DocumentModel[], void>({
            query: () => "/Document",
            providesTags: result =>
                result
                    ? [
                        ...result.map(docuemnt => ({ type: 'Document' as const, id: docuemnt.id })),
                        { type: 'Document', id: 'LIST' },
                    ]
                    : [{ type: 'Document', id: 'LIST' }],
        }),
        getDocumentById: builder.query<DocumentModel, string>({
            query: id => `/Document/${id}`,
            providesTags: result => result ? [{ type: 'Document', id: result.id }] : [],
        }),
    })
})

export const {
    useCreateDocumentMutation,
    useGetAllDocumentsQuery,
    useLazyGetDocumentByIdQuery,
} = DocumentApi;