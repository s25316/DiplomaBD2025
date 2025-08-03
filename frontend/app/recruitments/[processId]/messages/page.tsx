'use client';

import React, { useEffect, useState, useCallback } from 'react';
import { useParams, useRouter } from 'next/navigation';
import { useSession } from 'next-auth/react';
import Link from 'next/link';
import SelectItemsPerPage from '@/app/components/SelectItemsPerPage';
import Pagination from '@/app/components/Pagination';
import ReturnButton from '@/app/components/buttons/ReturnButton';

interface RecruitmentMessage {
  messageId: string;
  processId: string;
  message: string;
  created: string;
  isPersonSend: boolean; // True if message is from company, false if from person
}

interface SendMessagePayload {
  message: string;
}

const RecruitmentMessagesPage = () => {
  const { processId } = useParams() as { processId: string };
  const { data: session, status } = useSession();
  const router = useRouter();

  const [messages, setMessages] = useState<RecruitmentMessage[]>([]);
  const [error, setError] = useState<string | null>(null);
  const [loading, setLoading] = useState<boolean>(true);
  const [newMessageText, setNewMessageText] = useState<string>('');

  const [page, setPage] = useState(1);
  const [itemsPerPage, setItemsPerPage] = useState(10);
  const [totalCount, setTotalCount] = useState(0);

  const [isIndividual, setIsIndividual] = useState(true);
  useEffect(() => {
    fetch('http://localhost:8080/api/User')
      .then(res => res.json())
      .then(res => setIsIndividual(res.personPerspective.isIndividual))
  }, [])

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

  const fetchMessages = useCallback(async () => {
    if (status !== 'authenticated' || !session?.user?.token) {
      setLoading(false);
      return;
    }

    setLoading(true);
    setError(null);

    let apiUrl = 'http://localhost:8080/api/'
    if(!isIndividual){
      apiUrl += `CompanyUser/recruitments/${processId}/messages?Page=${page}&ItemsPerPage=${itemsPerPage}`
    }
    else{
      apiUrl += `User/recruitments/${processId}/messages?Page=${page}&ItemsPerPage=${itemsPerPage}`
    }

    try {
      const res = await fetch(
        apiUrl,
        {
          headers: {
            Authorization: `Bearer ${session.user.token}`,
            'Content-Type': 'application/json',
          },
          cache: 'no-store',
        }
      );

      if (!res.ok) {
        const errorText = await res.text();
        throw new Error(`Failed to fetch messages: ${errorText}`);
      }

      const data = await res.json();
      setMessages(data.items || []);
      setTotalCount(data.totalCount || 0);

    } catch (err: any) {
      console.error("Error fetching messages:", err);
      setError(err.message);
      showCustomAlert(`Error loading messages: ${err.message}`, true);
    } finally {
      setLoading(false);
    }
  }, [session, status, processId, page, itemsPerPage, isIndividual]);

  useEffect(() => {
    fetchMessages();
  }, [fetchMessages]);

  const handleSendMessage = async (e: React.FormEvent) => {
    e.preventDefault();
    if (status !== 'authenticated' || !session?.user?.token) {
      showCustomAlert("Authentication required to send message.", true);
      return;
    }
    if (!newMessageText.trim()) {
      showCustomAlert("Message cannot be empty.", true);
      return;
    }

    let apiUrl = 'http://localhost:8080/api/'
    if(!isIndividual){
      apiUrl += `CompanyUser/recruitments/${processId}/messages`
    }
    else{
      apiUrl += `User/recruitments/${processId}/messages`
    }

    try {
      const payload: SendMessagePayload = { message: newMessageText };
      const res = await fetch(
        apiUrl,
        {
          method: 'POST',
          headers: {
            Authorization: `Bearer ${session.user.token}`,
            'Content-Type': 'application/json',
          },
          body: JSON.stringify(payload),
        }
      );

      if (res.ok) {
        showCustomAlert("Message sent successfully!");
        setNewMessageText(''); 
        fetchMessages();
      } else {
        const errorText = await res.text();
        console.error("Failed to send message:", errorText);
        showCustomAlert(`Failed to send message: ${errorText}`, true);
      }
    } catch (err: any) {
      console.error("Error sending message:", err);
      showCustomAlert(`An unexpected error occurred while sending message: ${err.message}`, true);
    }
  };

  if (status === 'loading' || loading) {
    return <div className="text-center py-4 text-blue-600">Loading messages...</div>;
  }

  if (status === 'unauthenticated') {
    return <div className="text-center py-4 text-red-600">Unauthorized. Please log in.</div>;
  }

  if (error) {
    return <div className="text-center py-4 text-red-600">Error: {error}</div>;
  }

  return (
    <div className="max-w-4xl mx-auto p-6 bg-white dark:bg-gray-900 rounded-lg shadow-xl mt-8 font-inter text-gray-900 dark:text-gray-100">
      <ReturnButton />
      <h1 className="text-3xl font-bold mb-6 text-gray-800 dark:text-gray-100 text-center">Messages for Recruitment: {processId}</h1>
      
      <div className="mb-4">
        <Link href={`/recruitments/${processId}`} className="text-blue-600 hover:underline">
          &larr; Back to Recruitment Details
        </Link>
      </div>

      <p className="mb-4 text-sm text-gray-600 dark:text-gray-400">
        Showing {messages.length} of {totalCount} messages
      </p>

      <SelectItemsPerPage
        value={itemsPerPage}
        onChange={(val) => {
          setItemsPerPage(val);
          setPage(1);
        }}
      />

      <Pagination
        page={page}
        onPrev={() => setPage((prev) => Math.max(1, prev - 1))}
        onNext={() => setPage((prev) => prev + 1)}
        isNextDisabled={messages.length < itemsPerPage || (itemsPerPage * page) >= totalCount}
      />

      {messages.length > 0 ? (
        <ul className="space-y-4 mt-6">
          {messages.map((msg) => (
            <li key={msg.messageId} className={`p-4 rounded-lg shadow-sm ${msg.isPersonSend !== isIndividual ? 'bg-blue-100 dark:bg-gray-700 text-left' : 'bg-gray-100 dark:bg-blue-800 text-right'}`}>
              <p className="font-semibold">{msg.isPersonSend !== isIndividual ? !isIndividual ? 'Candidate' : "Company" : 'Me'}:</p>
              <p className="mt-1">{msg.message}</p>
              <p className="text-xs text-gray-500 dark:text-gray-400 mt-2">
                {new Date(msg.created).toLocaleString()}
              </p>
            </li>
          ))}
        </ul>
      ) : (
        <p className="text-gray-600 dark:text-gray-400 mt-6">No messages for this recruitment yet.</p>
      )}

      {/* Formularz do wysyłania wiadomości */}
      <div className="bg-gray-50 dark:bg-gray-800 p-6 rounded-lg shadow-inner mt-8">
        <h2 className="text-2xl font-bold mb-4 text-gray-800 dark:text-gray-100">Send New Message</h2>
        <form onSubmit={handleSendMessage} className="flex flex-col gap-4">
          <label htmlFor="newMessageTextarea" className="font-semibold text-gray-700 dark:text-gray-300">Your Message:</label>
          <textarea
            id="newMessageTextarea"
            value={newMessageText}
            onChange={(e) => setNewMessageText(e.target.value)}
            rows={5}
            placeholder="Type your message here..."
            className="border border-gray-300 dark:border-gray-600 rounded-md p-2 w-full bg-white dark:bg-gray-700 text-gray-900 dark:text-gray-100 focus:ring-blue-500 focus:border-blue-500"
          ></textarea>
          <button
            type="submit"
            className="bg-green-600 text-white px-5 py-2 rounded-lg hover:bg-green-700 transition duration-300 ease-in-out shadow-md font-semibold self-start"
          >
            Send Message
          </button>
        </form>
      </div>

      <Pagination
        page={page}
        onPrev={() => setPage((prev) => Math.max(1, prev - 1))}
        onNext={() => setPage((prev) => prev + 1)}
        isNextDisabled={messages.length < itemsPerPage || (itemsPerPage * page) >= totalCount}
      />
    </div>
  );
};

export default RecruitmentMessagesPage;