import { API_CONFIG, getApiUrl } from '~/config/api'
import type { FileYDto } from '~/types/fileYDto'
import type { ApiResponse } from '~/types/apiRespose'

export async function fetchFiles(page: number = 1
  , pageSize: number = 10
  , searchTerm?: string
  , sortBy:string = 'uploadedAt'
  , sortOrder: string = 'asc' ):Promise<ApiResponse<FileYDto[]>> {
    try {
      const response = await $fetch<FileYDto[]>(getApiUrl(API_CONFIG.ENDPOINTS.FILES.PAGE), {
        query: {
          page,
          pageSize,
          searchTerm,
          sortBy,
          sortOrder
        },
        credentials: 'include',
        mode: 'cors',
      })
      console.log("Response data.value = ", response);
      return {
        data: response || [],
        status: 200
      }
    } catch (error:any) {
      return {
        data: [],
        status: 500,
        error: error.data?.message || 'Failed to fetch files page'
      }
    }
}

export async function fetchFileDownload(url: string): Promise<ApiResponse<null>> {
  try {
    const response = await fetch(getApiUrl(API_CONFIG.ENDPOINTS.FILES.BY_URL) + url, {
      credentials: 'include',
      mode: 'cors',
    });

    if (!response.ok) {
      throw new Error('Failed to download file');
    }

    const blob = await response.blob();
    const fileUrl = URL.createObjectURL(blob);

    // Створюємо посилання
    const a = document.createElement('a');
    a.href = fileUrl;

    // Отримай ім’я файлу з заголовка або передай окремо
    const contentDisposition = response.headers.get('content-disposition');
    const filenameMatch = contentDisposition?.match(/filename="?([^"]+)"?/);
    const fileName = filenameMatch?.[1] || 'downloaded-file';

    a.download = fileName;
    document.body.appendChild(a);
    a.click();
    document.body.removeChild(a);

    // Очистити тимчасове посилання
    URL.revokeObjectURL(fileUrl);

    return {
      data: null,
      status: 200,
    };
  } catch (error: any) {
    return {
      data: null,
      status: 500,
      error: error.message || 'Failed to fetch file',
    };
  }
}

export async function fetchFileUpload(formData: FormData):Promise<ApiResponse<boolean>>{
try {
  const response = await $fetch(getApiUrl(API_CONFIG.ENDPOINTS.FILES.UPLOAD), {
    credentials: 'include',
    mode: 'cors',
    method: 'POST',
    body: formData,
  });
  return {
    data: true,
    status: 200
  }
} catch (error: any) {
  return {
    data: false,
    status: 500,
    error: error.message || 'Failed to upload file'
  }
}
}
export async function fetchFileCount():Promise<ApiResponse<number>>{
  try{
    const response = await $fetch<number>(getApiUrl(API_CONFIG.ENDPOINTS.FILES.COUNT), {
      credentials: 'include',
      mode: 'cors',
    });
    return {
      data: response,
      status: 200
    }
  }
  catch (error:any){
    return {
      data: 0,
      status: 500,
      error: error.message || 'Not Files Found'
    }
  }
}