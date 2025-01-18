import { getErrorMessage } from "@/helpers/error-handling";
import { getAcessToken } from "@/lib/actions/handle-acess-token";

export async function getPurchases(id: string) {
  const token = await getAcessToken();

  try {
    const response = await fetch(`${process.env.NEXT_PUBLIC_API_URL}/Anuncios/ListarCompras`, {
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${token}`
      },
    });
    
    const res = await response.json();
    
    return res;
  } catch (e: unknown) {
    console.log(e)
    return null;
 }
};

export async function buy(id: number, quantity: number) {
  const token = await getAcessToken();

  try {
    const response = await fetch(`${process.env.NEXT_PUBLIC_API_URL}/Anuncios/${id}/Compras?quantidade=${quantity}`, {
      method:"POST",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${token}`
      },
    });

    const res = await response.json();
    
    return res;
  } catch (e: unknown) {
    console.log(e)
    return null;
 }
}

export async function sendRating(userId: string, purchaseId: number,rating: number, role: "VENDEDOR" | "COMPRADOR") {

  const token = await getAcessToken();

  const path = `/${userId}/Compras/${purchaseId}` + (role === "VENDEDOR" ? `/AvaliarVendedor` : `/AvaliarComprador`);

    try {
    const response = await fetch(`${process.env.NEXT_PUBLIC_API_URL}/Anuncios` + path + `?nota=${rating}`, {
      method: 'PATCH',
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${token}`
      },
    });
    
    const res = await response.json();
    
    return res;
  } catch (e: unknown) {
    console.log(e)
    return null;
 }
}
