// import NextAuth, { AuthOptions } from "next-auth";
// import CredentialsProvider from "next-auth/providers/credentials";

// export const authOptions: AuthOptions = {
//   secret: process.env.AUTH_SECRET,
//   providers: [
//     CredentialsProvider({
//       name: "Credentials",
//       credentials: {
//         email: { label: "Email", type: "email" },
//         password: { label: "Password", type: "password" }
//       },
//       async authorize(credentials) {
//         const res = await fetch("http://localhost:8080/api/User/login", {
//           method: "POST",
//           headers: {
//             "Content-Type": "application/json"
//           },
//           body: JSON.stringify({
//             login: credentials?.email,
//             password: credentials?.password
//           }),
//         });

//         if (!res.ok) {
//           throw new Error("Login failed");
//         }

//         const user = await res.json();

//         if (user?.token) {
//           return {
//             ...user, // Przekazuje cały obiekt usera z backendu
//             token: user.token, // Upewniam się, że token istnieje
//           };
//         }

//         return null;
//       },
//     })
//   ],
//   callbacks: {
//     async jwt({ token, user }) {
//       if (user) {
//         token.accessToken = user.token;
//       }
//       return token;
//     },
//     async session({ session, token }) {
//       if (token?.accessToken) {
//         session.user.token = token.accessToken as string;
//       }
//       return session;
//     },
//   },
//   session: {
//     strategy: "jwt",
//   },
// };


// const handler = NextAuth(authOptions);

// export { handler as GET, handler as POST }


import NextAuth, { AuthOptions } from "next-auth";
import { JWT } from "next-auth/jwt";
import CredentialsProvider from "next-auth/providers/credentials";
import { permanentRedirect, useRouter, redirect } from "next/navigation";

const refreshToken = async (token: JWT) => {
  const res = await fetch(`http://localhost:8080/api/User/refreshToken`, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
      "Authorization": `Bearer ${token.accessToken}`
    },
    body: JSON.stringify({ "refreshToken": token.refreshToken })
  })

  const tokens = await res.json()

  return {
    accessToken: tokens.jwt,
    jwtValidTo: tokens.jwtValidTo,
    refreshToken: tokens.refreshToken, 
  }
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
        const res = await fetch("http://localhost:8080/api/User/login", {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
          },
          body: JSON.stringify({
            login: credentials?.email,
            password: credentials?.password
          }),
        });

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
          };
        }
        
        if(user?.isNeed2Stage){
          return {
            id: null,
            token: null,
            refreshToken: null,
            refreshTokenValidTo: null,
            jwtValidTo: null,
            isNeed2Stage: user.isNeed2Stage,
            user2StageData: user.user2StageData,
          }
        }

        return null;
      },
    })
  ],
  callbacks: {

    async jwt({ token, user }) {
      if(user?.isNeed2Stage){
        redirect(`/2stage/${user.user2StageData?.urlSegmentPart1}/${user.user2StageData?.urlSegmentPart2}`)
      }
      if (user?.token) {
        token.accessToken = user.token;
        token.jwtValidTo = user.jwtValidTo;
        token.refreshToken = user.refreshToken;
      }

      if (token.jwtValidTo && Date.now() < Date.parse(token.jwtValidTo)){
        return token;
      }

      return refreshToken(token);
    },
    async session({ session, token, user }) {
      if (token.accessToken) {
        session.user.token = token.accessToken as string;
      }
      return session;
    }
  },
};


const handler = NextAuth(authOptions);

export { handler as GET, handler as POST }