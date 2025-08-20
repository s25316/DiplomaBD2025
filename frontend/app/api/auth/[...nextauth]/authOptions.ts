import { AuthOptions } from "next-auth";
import { JWT } from "next-auth/jwt";
import CredentialsProvider from "next-auth/providers/credentials";
import { redirect } from "next/navigation";

const refreshToken = async (token: JWT) => {
  const res = await fetch(`${process.env.API_URL}/api/User/refreshToken`, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
      "Authorization": `Bearer ${token.accessToken}`
    },
    body: JSON.stringify({ "refreshToken": token.refreshToken })
  })

  const tokens = await res.json()

  const newJwt = {
    accessToken: tokens.jwt,
    refreshToken: tokens.refreshToken,
    jwtValidTo: tokens.jwtValidTo,
    refreshTokenValidTo: token.refreshTokenValidTo,
    isIndividual: token.isIndividual,
  }

  return newJwt
}

export const authOptions: AuthOptions = {
  secret: process.env.AUTH_SECRET,
  session:{
    strategy: "jwt"
  },
  providers: [
    CredentialsProvider({
      name: "Credentials",
      credentials: {
        email: { label: "Email", type: "email" },
        password: { label: "Password", type: "password" }
      },
      async authorize(credentials) {
        const res = await fetch(`${process.env.API_URL}/api/User/login`, {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
          },
          body: JSON.stringify({
            login: credentials?.email,
            password: credentials?.password
          }),
        });

        if(!res.ok){
          console.log(res.status)
          console.log(credentials )
        }

        const user = await res.json();

        if (user?.authorizationData?.jwt) {
          return {
            id: user.authorizationData.jwt, // wymagane pole `id`
            token: user.authorizationData.jwt,
            refreshToken: user.authorizationData.refreshToken,
            refreshTokenValidTo: user.authorizationData.refreshTokenValidTo,
            jwtValidTo: user.authorizationData.jwtValidTo,
            isNeed2Stage: user.isNeed2Stage,
            user2StageData: null,
            isIndividual: user.isIndividual,
          };
        }
        
        if(user?.isNeed2Stage){
          return {
            id: "2stage",
            token: null,
            refreshToken: null,
            refreshTokenValidTo: null,
            jwtValidTo: null,
            isNeed2Stage: user.isNeed2Stage,
            user2StageData: user.user2StageData,
            isIndividual: user.isIndividual,
          }
        }

        return null;
      },
    })
  ],
  debug: true,
  callbacks: {
    async jwt({ token, user }) {
      if(user?.isNeed2Stage){
        redirect(`/2stage/${user.user2StageData?.urlSegmentPart1}/${user.user2StageData?.urlSegmentPart2}`)
      }
      if (user?.token) {
        token.accessToken = user.token;
        token.jwtValidTo = user.jwtValidTo;
        token.refreshToken = user.refreshToken;
        token.isIndividual = user.isIndividual;
      }

      if (token.jwtValidTo && Date.now() < Date.parse(token.jwtValidTo)){
        return token;
      }

      return refreshToken(token);
    },
    async session({ session, token }) {
      if (token.accessToken) {
        session.user.token = token.accessToken as string;
      }
      return session;
    }
  },
}