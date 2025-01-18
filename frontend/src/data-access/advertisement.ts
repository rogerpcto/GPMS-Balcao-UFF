import { StatusType } from "@/app/meus-anuncios/page";
import { getErrorMessage } from "@/helpers/error-handling";
import { getAcessToken } from "@/lib/actions/handle-acess-token"

export async function getAds(filters?: string) {


  const path = `${process.env.NEXT_PUBLIC_API_URL}/Anuncios` + (filters ? `?${filters}` : '');

  try {
    const response = await fetch(path, {
        headers: {
          "Accept": "application/json",
          "Content-Type": "application/json",
        },
      }
    );
    
    const res =  await response.json();
    return res;
  } catch (e: unknown) {
    return null;
 }
}

export async function getMyAds(userId: string) {


  const path = `${process.env.NEXT_PUBLIC_API_URL}/Usuarios/${userId}/ListarAnuncios`;

  try {
    const response = await fetch(path, {
        headers: {
          "Accept": "application/json",
          "Content-Type": "application/json",
        },
      }
    );
    
    const res =  await response.json();
    return res;
  } catch (e: unknown) {
    return null;
 }
}

export async function getAd(id: string) {
  try {
    const response = await fetch(`${process.env.NEXT_PUBLIC_API_URL}/Anuncios/${id}`);
    const res = await response.json();
    return res;

  } catch (e: unknown) {
    alert(getErrorMessage(e));
    return null;
 }
}

export async function getCategoria() {
  try {
    const response = await fetch(`${process.env.NEXT_PUBLIC_API_URL}/Anuncios/Categorias`);
    const res = await response.json();
    return res;

  } catch (e: unknown) {
    alert(getErrorMessage(e));
    return null;
 }
}

export async function getTipoAnuncio() {
  try {
    const response = await fetch(`${process.env.NEXT_PUBLIC_API_URL}/Anuncios/TiposAnuncio`);
    const res = await response.json();
    return res;

  } catch (e: unknown) {
    alert(getErrorMessage(e));
    return null;
 }
}

export async function updateAdStatus(id: number, idCompra: number, status: StatusType) {

  const statusMap: Record<StatusType, string> = {
    AGUARDANDO_PAGAMENTO: "/AguardarPagamento",
    PRODUTO_RECEBIDO: "/AguardarPagamento",
    PAGAMENTO_EFETUADO: "/EfetuarPagamento",
    CONCLUIDO: "/Concluir",
    PAGAMENTO_CONFIRMADO: "/ConfirmarPagamento",
    NEGOCIANDO: "Negociando",
    COMPRADOR_AVALIADO: "",
    VENDEDOR_AVALIADO: ""
  };

  const token = await getAcessToken();

  try {
    const response = await fetch(`${process.env.NEXT_PUBLIC_API_URL}/Anuncios/${id}/Compras/${idCompra}` + statusMap[status], {
      method: "PATCH",
      headers: {
        "Content-Type": "application/json",
         Authorization: `Bearer ${token}`
      }
    });

    const res = await response.json();
    return res;

  } catch (e: unknown) {
    alert(getErrorMessage(e));
    return null;
 }
}

export async function editAd(id: string, data: any) {
  const token = await getAcessToken();

  try {
    const response = await fetch(`${process.env.NEXT_PUBLIC_API_URL}/Anuncios/${id}`, {
      method: "PUT", 
      headers: {
        "Content-Type": "application/json",
         Authorization: `Bearer ${token}`
      },
      body: JSON.stringify(data)
    });
    const res = await response.json();
    return res;

  } catch (e: unknown) {
    alert(getErrorMessage(e));
    return null;
 }
}

export async function disativateAd(id: string, data: any) {

  const token = await getAcessToken();
  
  try {
    const response = await fetch(`${process.env.NEXT_PUBLIC_API_URL}/Anuncios/${id}/Desativar`, {
      method: "PATCH", 
      headers: {
        "Content-Type": "application/json",
         Authorization: `Bearer ${token}`
      },
      body: JSON.stringify(data)
    });

    console.log("🚀 ~ disativateAd ~ response:", response)

    
    const res = await response.json();
    console.log("🚀 ~ disativateAd ~ res:", res)
    return res;

  } catch (e: unknown) {
    alert(getErrorMessage(e));
    return null;
 }
}

export async function sendMessage(id: number, idCompra: number, texto: string) {
  const token = await getAcessToken();

  try {
    const response = await fetch(`${process.env.NEXT_PUBLIC_API_URL}/Anuncios/${id}/Compras/${idCompra}/Mensagens?texto=${texto}`, {
      method: "POST", 
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${token}`
      },
      body: JSON.stringify({
        texto
      })
    });
    console.log("🚀 ~ sendMessage ~ response:", response);
    const res = await response.json();
    console.log("🚀 ~ sendMessage ~ res:", res)
    return res;

  } catch (e: unknown) {
    alert(getErrorMessage(e));
    return null;
 }
}

export async function sendImages(id: string, images: File) {
  const token = await getAcessToken();

  try {
    const formData = new FormData();

    formData.append(`file`, images);

    console.log("🚀 ~ sendImages ~ formData:", formData);

    const response = await fetch(`${process.env.NEXT_PUBLIC_API_URL}/Anuncios/${id}/Imagens`, {
      method: "POST",
      headers: {
        Authorization: `Bearer ${token}`
      },
      body: formData,
    });
    
    console.log("🚀 ~ sendImages ~ response:", response)
    // if (!response.ok) {
    //   throw new Error(`Erro na requisição: ${response.statusText}`);
    // }

    const res = await response.json();
    console.log("🚀 ~ sendImages ~ res:", res)
    return res;

  } catch (e: unknown) {
    alert(getErrorMessage(e));
    return null;
  }
}


export async function createAd(input: any) {

  const token = await getAcessToken();

  try {
    const response = await fetch(
      `${process.env.NEXT_PUBLIC_API_URL}/Anuncios`,
      {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${token}`
        },
        body: JSON.stringify(input),
      }
    );

    const res = await response.json();

    return res;
  } catch (error) {
    console.log("🚀 ~ createAd ~ error:", error)
    return null;
  }
}