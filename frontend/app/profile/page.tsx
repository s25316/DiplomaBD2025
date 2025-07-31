'use client';

import React, { useEffect, useState } from 'react';
import { useSession, signOut } from 'next-auth/react';
import { useRouter } from 'next/navigation';
import Link from 'next/link';
import CreateCompanyButton from '@/app/components/buttons/CreateCompanyButton';
import BaseProfileForm from '../components/forms/BaseProfileForm';
import { InnerSection, OuterContainer } from '../components/layout/PageContainers';

interface Company{
  companyId: string;
  name: string;
  description: string;
  websiteUrl?: string;
}
interface SkillInfo {
  name: string;
  skillType: {
    name: string;
  };
}

interface LinkInfo {
  value: string,
  urlType: {
    urlTypeId: number;
    name: string;
  }
}
interface Address {
  streetName: string;
  houseNumber: string;
  apartmentNumber: string | null;
  postCode: string;
  cityName: string;
  countryName: string;
}
interface User {
  logo: string | null;
  name: string;
  surname: string;
  isIndividual: boolean;
  isTwoFactorAuth: boolean;
  isStudent: boolean;
  contactEmail: string;
  phoneNum: string;
  birthDate: string;
  address: Address;
  skills: SkillInfo[];
  urls: LinkInfo[];
}

const Profile = () => {
  const { data: session, status } = useSession(); 
  const router = useRouter();

  const [user, setUserDetails] = useState<User | null>(null);
  const [companies, setCompanies] = useState<Company[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null); 
  const [isDeleting, setIsDeleting] = useState(false); 

  const showCustomAlert = (message: string, isError: boolean = false) => {
    let alertMessage = message;
    if (isError) {
      try {
        const errorJson = JSON.parse(message);
        if (errorJson.errors) {
          alertMessage = "Validation Errors:\n" + Object.entries(errorJson.errors).map(([key, value]) => `${key}: ${(value as string[]).join(", ")}`).join("\n");
        } else if (errorJson.title) {
          alertMessage = errorJson.title + (errorJson.detail ? `\n${errorJson.detail}` : "");
        } else {
          alertMessage = message;
        }
      } catch (e) {
        alertMessage = message;
      }
    }
    console.log(isError ? "ERROR ALERT:" : "ALERT:", alertMessage);
    alert(alertMessage);
  };

  useEffect(() => {
    const fetchData = async () => {
      if (status !== 'authenticated' || !session?.user?.token) {
        setLoading(false);
        return;
      }

      setLoading(true);
      setError(null);

      try {
        const [userRes, companiesRes] = await Promise.all([
          fetch('http://localhost:8080/api/User', {
            headers: {
              Authorization: `Bearer ${session.user.token}`,
            },
            cache: 'no-store',
          }),
          fetch('http://localhost:8080/api/CompanyUser/companies', {
            headers: {
              Authorization: `Bearer ${session.user.token}`,
              'Content-Type': 'application/json',
            },
            cache: 'no-store',
          }),
        ]);

        if (!userRes.ok) {
          const errorText = await userRes.text();
          throw new Error(`Failed to fetch user details: ${userRes.status} ${userRes.statusText} - ${errorText}`);
        }
        if (!companiesRes.ok) {
          const errorText = await companiesRes.text();
          throw new Error(`Failed to fetch companies: ${companiesRes.status} ${companiesRes.statusText} - ${errorText}`);
        }

        const userJson = await userRes.json();
        const companiesJson = await companiesRes.json();

        setUserDetails(userJson.personPerspective);
        setCompanies(companiesJson.items || []);
      } catch (err: any) {
        console.error('Error loading profile:', err);
        setError(err.message);
        showCustomAlert(`Error loading profile: ${err.message}`, true);
      } finally {
        setLoading(false);
      }
    };

    fetchData();
  }, [session, status]);

  const handleDeleteAccount = async () => {
    if (!session?.user?.token) {
      showCustomAlert("Authentication required to delete account.", true);
      return;
    }

    const confirmationMessage = `Are you sure you want to delete your account? This action is irreversible after 30 days. You will be logged out immediately.`;
    if (!window.confirm(confirmationMessage)) {
      return;
    }

    setIsDeleting(true);
    setError(null);

    try {
      const res = await fetch('http://localhost:8080/api/User', {
        method: 'DELETE',
        headers: {
          'Authorization': `Bearer ${session.user.token}`,
        },
      });

      if (!res.ok) {
        const errorText = await res.text();
        throw new Error(`Failed to delete account: ${res.status} ${res.statusText} - ${errorText}`);
      }

      showCustomAlert('Your account has been marked for deletion. You have 30 days to restore it. You will now be logged out.');
      await signOut({ callbackUrl: '/' }); // Wyloguj użytkownika po usunięciu konta
    } catch (err: any) {
      console.error("Error deleting account:", err);
      setError(err.message);
      showCustomAlert(`Error deleting account: ${err.message}`, true);
    } finally {
      setIsDeleting(false);
    }
  };

  if (status === 'loading' || loading) {
    return <div className="text-center py-4 text-blue-600">Loading profile...</div>;
  }
  if (status === 'unauthenticated') {
    return <div className="text-center py-4 text-red-600">Unauthorized. Please log in.</div>;
  }
  if (error) {
    return <div className="text-center py-4 text-red-600">Error: {error}</div>;
  }
  if (!user) {
    return <div className="text-center py-4 text-gray-600">Profile data not found.</div>;
  }

  const {skills, urls, address} = user;
  const isFirstTime = !user?.name;
  
  return (
    <OuterContainer maxWidth="max-w-4xl">
      <h1 className="text-3xl font-bold mb-6 text-gray-800 dark:text-gray-100 text-center">My Profile</h1>
      
      <InnerSection className="mb-6">
        {isFirstTime ? (
          <BaseProfileForm
            token={session?.user.token!}
            onSuccess={() => window.location.reload()} 
          />
        ) : (
          <div className="space-y-2 text-gray-700 dark:text-gray-300">
            <h2 className="text-xl font-semibold mb-2 text-gray-800 dark:text-gray-100">
              <b> {user.isIndividual ? 'Individual account' : 'Company account'} </b>
            </h2>
            <p><b>Name:</b> {user.name}</p>
            <p><b>Surname:</b> {user.surname}</p>
            <p><b>Email:</b> {user.contactEmail}</p>
            <p><b>Phone:</b> {user.phoneNum}</p>
            <p><b>Birth Date:</b> {user.birthDate?.substring(0, 10)}</p>

            {(address !=null) && (
              <p><b>Address:</b> 
                {[
                  " ul.",
                  address.streetName,
                  address.houseNumber, "/",
                  address.apartmentNumber, ",",
                  address.postCode, ",",
                  address.cityName,
                  address.countryName,
                ]
                  .filter(Boolean)
                  .join(' ')}
              </p>
            )}

            <p className="mt-4"><b>Skills:</b></p>
            <ul className="list-disc ml-5">
              {skills?.length > 0 ? (
                skills.map((s: any) => (
                  <li key={s.skillId}>{s.name} ({s.skillType?.name})</li>
                ))
              ) : (
                <li>No skills added.</li>
              )}
            </ul>

            <p className="mt-4"><b>Links:</b></p>
            <ul className="list-disc ml-5">
              {urls?.length > 0 ? (
                urls.map((u: any, i: number) => (
                  <li key={i}><a href={u.value} target="_blank" rel="noopener noreferrer" className="text-blue-600 hover:underline dark:text-blue-400">{u.value}</a> ({u.urlType?.name})</li>
                ))
              ) : (
                <li>No links added.</li>
              )}
            </ul>

            <button
              onClick={() => router.push('/profile/edit')}
              className="inline-block bg-blue-600 text-white px-5 py-2 rounded-lg hover:bg-blue-700 transition duration-300 ease-in-out shadow-md font-semibold mt-6"
            >
              Edit Profile
            </button>
          </div>
        )}
      </InnerSection>

      {(user.isIndividual === false && user.name != null) && (
        <InnerSection className="mb-6"> 
          <h2 className="text-2xl font-semibold mt-4 mb-2 text-gray-800 dark:text-gray-100">Companies</h2> {/* Zmieniono nagłówek */}
          {companies.length > 0 ? (
            <ul className="space-y-4">
              {companies.map((company) => (
                <li key={company.companyId} >
                  <div className="border border-gray-300 dark:border-gray-700 p-4 rounded-lg shadow-sm bg-white dark:bg-gray-800 text-gray-900 dark:text-gray-100"> {/* Dodano style dla pojedynczej firmy */}
                    <Link href={`/companies/${company.companyId}`} className="text-blue-600 hover:underline dark:text-blue-400">
                      <b className="text-lg font-bold">{company.name}</b> 
                    </Link>
                    {company.websiteUrl && <p className="text-sm text-gray-700 dark:text-gray-300">Website: <a href={company.websiteUrl} target="_blank" rel="noopener noreferrer" className="text-blue-600 hover:underline dark:text-blue-400">{company.websiteUrl}</a></p>}
                  </div>
                </li>
              ))}
            </ul>
          ) : (
            <p className="text-gray-600 dark:text-gray-400">You did not register any company yet.</p> 
          )}
          <div className="mt-6">
            <CreateCompanyButton />
          </div>
        </InnerSection>
      )}

      <InnerSection>
        <h2 className="text-2xl font-semibold mb-4 text-gray-800 dark:text-gray-100">Account Management</h2>
        <p className="text-red-600 dark:text-red-400 font-bold mb-4">
          WARNING: Deleting your account is a permanent action after 30 days. You can restore it within this period.
        </p>
        <button
          onClick={handleDeleteAccount}
          disabled={isDeleting}
          className="inline-block bg-red-600 text-white px-5 py-2 rounded-lg hover:bg-red-700 transition duration-300 ease-in-out shadow-md font-semibold disabled:opacity-50 disabled:cursor-not-allowed"
        >
          {isDeleting ? 'Deleting Account...' : 'Delete Account'}
        </button>
      </InnerSection>
    </OuterContainer>
  );
};

export default Profile;
