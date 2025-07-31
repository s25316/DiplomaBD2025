'use client';
import { useParams, useRouter } from "next/navigation";
import { useEffect, useState, useRef } from "react";
import { useSession } from "next-auth/react";
import BranchForm from "@/app/components/forms/BranchForm"; // Import BranchForm
import CancelButton from "@/app/components/buttons/CancelButton";
import { OuterContainer } from "@/app/components/layout/PageContainers";

// Represents the address structure as sent to/from the API
interface AddressData {
  countryName: string;
  stateName: string;
  cityName: string;
  streetName: string | null;
  houseNumber: string;
  apartmentNumber: string | null;
  postCode: string;
  lon: number;
  lat: number;
}

// Represents the branch data as it comes from the API (for `setBranch` state)
interface BranchApiData {
  branchId: string; // Add branchId from API
  name: string;
  description: string | null;
  address: AddressData;
}

// Represents the data structure sent to the BranchForm component (similar to BranchFormOutputData)
interface BranchFormInitialData {
  index: number;
  name: string;
  description: string | null;
  address: AddressData;
}

// Represents the payload sent to the PUT /branches API endpoint
interface BranchUpdatePayload {
  name: string;
  description: string | null;
  address: AddressData;
}

const EditBranch = () => {
  const { data: session } = useSession();
  // Ensure useParams returns string types
  const { id, branchId } = useParams() as { id: string; branchId: string };
  const router = useRouter();

  // State to hold the fetched branch data (optional, can be directly mapped to formState)
  const [branch, setBranch] = useState<BranchApiData | null>(null);
  // State to hold the data that will be passed as initialData to BranchForm
  const [formState, setFormState] = useState<BranchFormInitialData | null>(null);
  // useRef to store the data from BranchForm for submission
  const sendData = useRef<BranchFormInitialData | null>(null); // Type this ref correctly

  // Custom alert function
  const showCustomAlert = (message: string) => {
    console.log("ALERT:", message);
    alert(message);
  };

  useEffect(() => {
    if (!session?.user?.token || !branchId) return;

    const fetchBranch = async () => {
      try {
        console.log(`Fetching branch details for branchId: ${branchId}`); // Debug log
        const res = await fetch(`http://localhost:8080/api/CompanyUser/branches/${branchId}`, {
          headers: {
            Authorization: `Bearer ${session.user.token}`,
          },
        });

        if (!res.ok) {
          const errorText = await res.text();
          throw new Error(`Failed to fetch branch details: ${errorText}`);
        }

        const data: { items: { branch: BranchApiData }[] } = await res.json(); // Explicitly type API response
        const b = data.items?.[0]?.branch;

        if (!b) {
          showCustomAlert("Branch not found or malformed data.");
          router.push(`/companies/${id}`); // Redirect to company page if branch not found
          return;
        }

        setBranch(b); // Store the raw API branch data

        // Prepare data for BranchForm's initialData prop
        const initialForm: BranchFormInitialData = {
          index: 0, // Assuming a single form for editing
          name: b.name,
          description: b.description,
          address: { ...b.address }, // Deep copy address to prevent direct mutation
        };
        setFormState(initialForm);
        sendData.current = initialForm; // Also initialize ref with the same data
        console.log("Fetched branch and set formState/sendData:", initialForm); // Debug log

      } catch (error: any) {
        console.error("Error fetching branch:", error);
        showCustomAlert(`Failed to load branch details: ${error.message}`);
      }
    };

    fetchBranch();
  }, [session, branchId, id, router]); // Add router to dependencies

  // Callback function to receive updated data from BranchForm
  const handleData = (data: BranchFormInitialData) => {
    sendData.current = data;
    console.log("handleData: Received data from BranchForm:", data); // Debug log
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!session?.user?.token || !sendData.current) {
      showCustomAlert("Authentication required or branch data missing.");
      return;
    }

    const payload: BranchUpdatePayload = { // Type the payload
      name: sendData.current.name,
      description: sendData.current.description,
      address: {
        ...sendData.current.address, // Ensure all address fields are included
      },
    };

    try {
      console.log("handleSubmit: Submitting payload:", payload); // Debug log
      const res = await fetch(`http://localhost:8080/api/CompanyUser/companies/branches/${branchId}`, {
        method: "PUT",
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${session?.user.token}`,
        },
        body: JSON.stringify(payload),
      });

      if (res.ok) {
        showCustomAlert("Branch updated successfully!");
        router.push(`/companies/${id}/${branchId}`); // Redirect to the branch details page
      } else {
        const errorText = await res.text();
        console.error("Failed to update branch:", errorText);
        showCustomAlert(`Failed to update branch: ${errorText}`);
      }
    } catch (error: any) {
      console.error("Error updating branch:", error);
      showCustomAlert(`An error occurred while updating the branch: ${error.message}`);
    }
  };

  // Render loading state if formState is not yet available
  if (!formState) return <div className='text-center py-4 text-blue-600'>Loading branch data...</div>;
  if (!session?.user?.token) return <div className='text-center py-4 text-red-600'>Unauthorized. Please log in.</div>;


  return (
    <OuterContainer>
      <h1 className="text-3xl font-bold mb-6 text-gray-800 dark:text-gray-100 text-center">Edit Branch</h1>
      <form className='flex flex-col gap-6' onSubmit={handleSubmit}>
        <BranchForm index={0} getData={handleData} initialData={formState} />
        <button
          type='submit'
          className='bg-blue-600 text-white px-5 py-2 rounded-lg hover:bg-blue-700 transition duration-300 ease-in-out shadow-md font-semibold mt-4'
        >
          Save Changes
        </button>
        <CancelButton/>
      </form>
    </OuterContainer>
  );
};

export default EditBranch;