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
import CredentialsProvider from "next-auth/providers/credentials";

export const authOptions: AuthOptions = {
  secret: process.env.AUTH_SECRET,
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
            "Content-Type": "application/json"
          },
          body: JSON.stringify({
            login: credentials?.email,
            password: credentials?.password
          }),
        });

        if (!res.ok) {
          throw new Error("Login failed");
        }

        const user = await res.json();

        // if (user?.token) {
        //   return {
        //     ...user, // Przekazuje cały obiekt usera z backendu
        //     token: user.token, // Upewniam się, że token istnieje
        //   };
        // }
        if (user?.authorizationData?.jwt) {
          return {
            id: user.authorizationData.jwt, // wymagane pole `id`
            token: user.authorizationData.jwt,
            refreshToken: user.authorizationData.refreshToken,
            refreshTokenValidTo: user.authorizationData.refreshTokenValidTo,
            jwtValidTo: user.authorizationData.jwtValidTo,
          };
        }
        

        return null;
      },
    })
  ],
  callbacks: {
    // async jwt({ token, user }) {
    //   if (user) {
    //     token.accessToken = user.token;
    //   }
    //   return token;
    // },
    async jwt({ token, user }) {
      if (user) {
        token.accessToken = user.token;
        token.refreshToken = user.refreshToken;
        token.jwtValidTo = user.jwtValidTo;
      }
      return token;
    },
    async session({ session, token }) {
      if (token?.accessToken) {
        session.user.token = token.accessToken as string;
      }
      return session;
    }
    
    // async session({ session, token }) {
    //   if (token?.accessToken) {
    //     session.user.token = token.accessToken as string;
    //   }
    //   return session;
    // },
  },
  session: {
    strategy: "jwt",
  },
};


const handler = NextAuth(authOptions);

export { handler as GET, handler as POST }